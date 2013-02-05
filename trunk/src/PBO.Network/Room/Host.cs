using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PokemonBattleOnline.Tactic;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Network.Room
{
  public class Host : IHost, INotifyPropertyChanged, IDisposable
  {
    private static readonly PropertyChangedEventArgs STATE = new PropertyChangedEventArgs("State");
    public static readonly PropertyChangedEventArgs CAN_START_GAME = new PropertyChangedEventArgs("CanStartGame");
    
    public event PropertyChangedEventHandler PropertyChanged = delegate { };
    public event Action Closed;
    internal readonly GameInitSettings GameSettings;
    private readonly bool auto;
    
    private readonly Dispatcher dispatcher;
    
    private readonly HashSet<int> users;
    private readonly ObservableCollection<Player> players;
    private readonly ObservableCollection<int> spectators;
    private readonly InitingGame initingGame;
    
    private IGame game;
    private GoTimer timer;
    
    /// <param name="auto">游戏自动开始，游戏结束时自动关闭房间</param>
    public Host(GameInitSettings settings, bool auto)
    {
      dispatcher = new Dispatcher("Host", true);
      users = new HashSet<int>();
      
      players = new ObservableCollection<Player>();
      Players = new ReadOnlyObservableCollection<Player>(players);
      spectators = new ObservableCollection<int>();
      Spectators = new ReadOnlyObservableCollection<int>(spectators);
      
      initingGame = new InitingGame(settings);
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
    { get { return initingGame.CanComplete; } }
    private void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      PropertyChanged(this, e);
    }

    #region Control
    private void EndGame()
    {
      State = RoomState.GameEnd;
      if (timer != null) timer.Dispose();
    }
    public void Kick(int targetId)
    {
    }
    public void StartGame()
    {
      if (CanStartGame)
      {
        State = RoomState.GameStarted;
        timer = new GoTimer(from p in players select p.Id);
        timer.TimeUp += InformTimeUp;
        timer.WaitingNotify += InformWaitingForInput;
        timer.Start();
        game = initingGame.Complete();
        game.ReportUpdated += InformReportUpdate;
        foreach (var t in game.Teams)
          foreach (var p in t.Players)
          {
            var parner = GameSettings.Mode.PlayersPerTeam() == 1 ? null : initingGame.GetPokemons(t.Id, 1 - p.IndexInTeam);
            InformPlayerInfo(p.Id, p.IndexInTeam, parner);
          }
        game.Start();
      }
    }
    public void CloseRoom()
    {
      EndGame();
      Closed();
    }
    #endregion

    #region Commands
    void IHost.ExecuteCommand(HostCommand command, int senderId)
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
    void IRoomManager.JoinGame(int userId, IPokemonData[] pokemons, int teamId)
    {
      bool canStartGame = CanStartGame;
      if (initingGame.SetPlayer(teamId, userId, pokemons))
      {
        users.Add(userId);
        players.Add(new Player(userId, teamId));
        InformEnterSucceed(userId, teamId);
        OnPropertyChanged(null);
        if (CanStartGame && auto) StartGame();
      }
      else InformEnterFailed("debug.failed", userId);
    }
    void IRoomManager.SpectateGame(int userId)
    {
      spectators.Add(userId);
      users.Add(userId);
      InformEnterSucceed(userId, -1);
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
          if (State == RoomState.GameStarted) InformGameStop(userId, GameStopReason.PlayerGiveUp);
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
        if (game.GetPlayer(userId) != null && game.InputAction(userId, action))
        {
          timer.Pause(userId);
          game.TryContinue();
        }
        else InformGameStop(userId, GameStopReason.InvalidInput);
    }
    #endregion

    #region Inform
    internal event Action<UserInformation, int[]> SendInformation;
    void OnSendInformation(UserInformation info, params int[] userIds)
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
    void InformEnterSucceed(int userId, int teamId)
    {
      if (teamId != -1)
      {
        OnSendInformation(new UserJoinGameInfo(userId, teamId));
        OnSendInformation(EnterSucceedInfo.Player(this), userId);
      }
      else
      {
        OnSendInformation(new UserSpectateGameInfo(userId));
        OnSendInformation(EnterSucceedInfo.Spectator(this, game.GetLastLeapFragment()), userId);
      }
    }
    #endregion

    #region Game Informer
    void InformPlayerInfo(int playerId, int teamIndex, IPokemonData[] pokemons)
    {
      OnSendInformation(new PlayerInfo(teamIndex, pokemons), playerId);
    }
    /// <summary>
    /// 议和，不经过Game
    /// </summary>
    void InformGameTie()
    {
      EndGame();
      OnSendInformation(GameEndInfo.GameTie());
    }
    void InformGameStop(int userId, GameStopReason reason)
    {
      EndGame();
      OnSendInformation(GameEndInfo.GameStop(userId, reason));
    }
    void InformTimeUp()
    {
      EndGame();
      OnSendInformation(GameEndInfo.TimeUp(timer.State));
    }
    void InformWaitingForInput(IEnumerable<int> players)
    {
      OnSendInformation(new WaitingForInputInfo(players));
    }

    void InformReport()
    {
    }
    private int lastTurn;
    void InformReportUpdate(ReportFragment fragment, IDictionary<int, InputRequest> requirements)
    {
      if (requirements != null)
      {
        timer.NewTurns(game.Turn - lastTurn);
        lastTurn = game.Turn;
        foreach (var pair in requirements)
          OnSendInformation(new RequireInputInfo(pair.Value, timer.GetState(pair.Key)), pair.Key);
        timer.Resume(requirements.Keys);
      }
      OnSendInformation(new ReportUpdateInfo(fragment));
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
