using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using LightStudio.Tactic;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  public class Host : IHost, INotifyPropertyChanged, IDisposable
  {
    private static readonly PropertyChangedEventArgs STATE = new PropertyChangedEventArgs("State");
    public static readonly PropertyChangedEventArgs CAN_START_GAME = new PropertyChangedEventArgs("CanStartGame");
    
    public event PropertyChangedEventHandler PropertyChanged = delegate { };
    public event Action Closed;
    internal readonly GameInitSettings GameSettings;
    private readonly Dispatcher dispatcher;
    private readonly Timer timer;
    private readonly HashSet<int> users;
    private readonly ObservableCollection<Player> players;
    private readonly ObservableCollection<int> spectators;
    private readonly IGame game;
    private readonly bool auto;
    
    /// <param name="auto">游戏自动开始，游戏结束时自动关闭房间</param>
    public Host(GameInitSettings settings, bool auto)
    {
      dispatcher = new Dispatcher("Host", true);
      timer = new Timer(TimeTick, null, Timeout.Infinite, 1000);
      users = new HashSet<int>();
      
      players = new ObservableCollection<Player>();
      Players = new ReadOnlyObservableCollection<Player>(players);
      spectators = new ObservableCollection<int>();
      Spectators = new ReadOnlyObservableCollection<int>(spectators);
      
      game = GameFactory.CreateGame(settings, settings.NextId);
      game.ReportUpdated += InformReportUpdate;
      GameSettings = settings;
      this.auto = auto;
    }

    public ReadOnlyObservableCollection<int> Spectators
    { get; private set; }
    public ReadOnlyObservableCollection<Player> Players
    { get; private set; }
    private RoomState _state;
    public RoomState State
    {
      get { return _state; }
      private set
      {
        if (_state != value)
        {
          _state = value;
          OnPropertyChanged(STATE);
        }
      }
    }
    public bool CanStartGame
    { get { return game.Prepared; } }
    private void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      PropertyChanged(this, e);
    }
    private void TimeTick(object state)
    {
      foreach (Player p in players)
        if (p.IsInputing) p.Tick();
      var timeouter = (from p in players where !p.Alive select p).ToArray();
      if (timeouter.Length != 0) InformTimeUp();
    }

    #region Control
    public void Kick(int targetId)
    {
    }
    public void StartGame()
    {
      if (game.Start())
      {
        State = RoomState.GameStarted;
        timer.Change(0, 1000);
      }
    }
    public void CloseRoom()
    {
      Closed();
    }
    #endregion

    #region Commands
    void IHost.ExecuteCommand(IHostCommand command, int senderId)
    {
      dispatcher.Invoke(() =>
      {
        try
        {
          command.Execute(this, senderId);
        }
        catch
        {
          System.Diagnostics.Debugger.Break();
        }
      });
    }
    void IRoomManager.JoinGame(int userId, PokemonCustomInfo[] pokemons, int teamId)
    {
      bool canStartGame = CanStartGame;
      if (game.SetPlayer(teamId, userId, pokemons))
      {
        users.Add(userId);
        players.Add(new Player(userId, teamId));
        InformEnterSucceed(userId, true);
        OnPropertyChanged(null);
        if (CanStartGame && auto) StartGame();
      }
      else
        InformEnterFailed("debug.failed", userId);
    }
    void IRoomManager.SpectateGame(int userId)
    {
      spectators.Add(userId);
      users.Add(userId);
      InformEnterSucceed(userId, false);
    }
    void IRoomManager.Quit(int userId)
    {
      if (users.Remove(userId))
      {
        InformUserQuit(userId);
        if (!spectators.Remove(userId))
        {
          foreach (Player p in players)
            if (p.Id == userId)
            {
              players.Remove(p);
              break;
            }
          if (State == RoomState.GameStarted) InformGameStop(userId, GameStopReason.UserQuit);
        }
        if (auto && users.Count == 0) CloseRoom();
      }
    }
    void IGameManager.RequestTie(int userId)
    {
      if (State == RoomState.GameStarted) ;
    }
    void IGameManager.RejectTie(int userId)
    {
      if (State == RoomState.GameStarted) ;
    }
    void IGameManager.AcceptTie(int userId)
    {
      if (State == RoomState.GameStarted) ;
    }
    void IGameManager.Input(int userId, ActionInput action)
    {
      if (State == RoomState.GameStarted)
        if (game.InputAction(userId, action)) game.TryContinue();
        else InformGameStop(userId, GameStopReason.InvalidInput);
    }
    #endregion

    #region Inform
    internal event Action<IUserInformation, int[]> SendInformation;
    void OnSendInformation(IUserInformation info, params int[] userIds)
    {
      if (userIds.Length == 0) SendInformation(info, users.ToArray());
      else SendInformation(info, userIds);
    }

    #region Messaging.Room Informer
    void InformUserSpectateGame(int userId)
    {
    }
    void InformUserJoinGame(int userId, int teamId)
    {
    }
    void InformUserQuit(int userId)
    {
      OnSendInformation(new UserQuitInfo(userId));
    }
    void InformUserKicked(int userId)
    {
      OnSendInformation(new UserKickedInfo(userId));
    }
    void InformEnterFailed(string message, int userId)
    {
      OnSendInformation(new EnterFailedInfo(message), userId);
    }
    void InformEnterSucceed(int userId, bool isPlayer)
    {
      if (isPlayer)
      {
        Game.Player p = game.GetPlayer(userId);
        int[] ids = new int[p.Pokemons.Count()];
        {
          int i = -1;
          foreach (Pokemon pm in p.Pokemons) ids[++i] = pm.Id;
        }
        OnSendInformation(new UserJoinGameInfo(userId, p.Team.Id));
        OnSendInformation(EnterSucceedInfo.Player(this, ids), userId);
      }
      else
      {
        OnSendInformation(new UserSpectateGameInfo(userId));
        OnSendInformation(EnterSucceedInfo.Spectator(this, game.GetLastLeapFragment()), userId);
      }
    }
    #endregion

    #region Game Informer
    /// <summary>
    /// 正常结束 至少有一方精灵全部倒下
    /// </summary>
    void InformGameResult(int team0, int team1)
    {
      State = RoomState.GameEnd;
      OnSendInformation(GameEndInfo.GameResult(team0, team1));
    }
    /// <summary>
    /// 议和，不经过Game
    /// </summary>
    void InformGameTie()
    {
      State = RoomState.GameEnd;
      OnSendInformation(GameEndInfo.GameTie());
    }
    void InformGameStop(int userId, GameStopReason reason)
    {
      State = RoomState.GameEnd;
      OnSendInformation(GameEndInfo.GameStop(userId, reason));
    }
    void InformTimeUp()
    {
      State = RoomState.GameEnd;
      timer.Change(Timeout.Infinite, 0);
      OnSendInformation(GameEndInfo.TimeUp(from p in players select p.Seconds));
    }

#if !DEBUG
    private Dictionary<int, RequireInput> lastRequirements;
#endif
    private int lastTurn;
    void InformReportUpdate(ReportFragment fragment, IEnumerable<KeyValuePair<int, InputRequest>> requirements)
    {
      bool hasAddition;
#if DEBUG
      hasAddition = true;
#else
      if (lastRequirements.Values.First() != null)
      {
        hasAddition = false;
        foreach (KeyValuePair<int, RequireInput> pair in requirements)
          if (!pair.Value.Equals(lastRequirements.ValueOrDefault(pair.Key)))
          {
            hasAddition = true;
            break;
          }
      }
#endif
      foreach (Player p in players) p.NewTurns(game.Turn - lastTurn);
      lastTurn = game.Turn;
      if (hasAddition)
        foreach(KeyValuePair<int, InputRequest> pair in requirements) OnSendInformation(new RequireInputInfo(pair.Value), pair.Key);
      OnSendInformation(new ReportUpdateInfo(fragment, hasAddition));
    }

    void InformRequestTie()
    {
    }
    void InformTieRejected()
    {
    }
    #endregion
    #endregion

    public void Dispose()
    {
      PropertyChanged = delegate { };
      SendInformation = delegate { };
      timer.Dispose();
      dispatcher.Dispose();
    }
  }
}
