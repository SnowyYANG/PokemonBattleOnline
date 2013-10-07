using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network.Room
{
  public class Host : IHost, INotifyPropertyChanged, IDisposable
  {
    private static readonly PropertyChangedEventArgs STATE = new PropertyChangedEventArgs("State");
    public static readonly PropertyChangedEventArgs CAN_START_GAME = new PropertyChangedEventArgs("CanStartGame");
    
    public event PropertyChangedEventHandler PropertyChanged = delegate { };
    public event Action Closed;
    internal readonly GameInitSettings GameSettings;
    private readonly bool Auto;
    
    private readonly HashSet<int> users;
    private readonly ObservableCollection<Player> players;
    
    private InitingGame initingGame;
    private IGame game;
    private GoTimer timer;
    
    /// <param name="auto">游戏自动开始，游戏结束时自动关闭房间</param>
    public Host(GameInitSettings settings, bool auto)
    {
      users = new HashSet<int>();
      
      players = new ObservableCollection<Player>();
      Players = new ReadOnlyObservableCollection<Player>(players);
      
      initingGame = new InitingGame(settings);
      GameSettings = settings;
    }

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

    private void EndGame()
    {
      State = RoomState.Available;
      if (timer != null) timer.Dispose();
    }
    private void StartGame()
    {
      if (CanStartGame)
      {
        State = RoomState.Battling;
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
    private void CloseRoom()
    {
      EndGame();
      Closed();
    }

    private readonly object locker = new object();
    void IHost.ExecuteCommand(HostCommand command, int senderId)
    {
      lock (locker)
      {
        command.Execute(this, senderId);
      }
    }
    void IHost.JoinGame(int userId, IPokemonData[] pokemons, int teamId)
    {
      bool canStartGame = CanStartGame;
      if (initingGame.SetPlayer(teamId, userId, pokemons))
      {
        users.Add(userId);
        players.Add(new Player(userId, teamId));
        InformEnterSucceed(userId, teamId);
        OnPropertyChanged(null);
        if (CanStartGame && Auto) StartGame();
      }
      else InformEnterFailed("debug.failed", userId);
    }
    void IHost.SpectateGame(int userId)
    {
      if (users.Contains(userId))
      {
        users.Add(userId);
        System.Diagnostics.Debugger.Break();
        //InformEnterSucceed(userId, -1); 
      }
    }
    void IHost.Quit(int userId)
    {
      if (users.Remove(userId))
      {
        InformUserQuit(userId);
        foreach (Player p in players)
          if (p.Id == userId)
          {
            players.Remove(p);
            if (State == RoomState.Battling) InformGameStop(userId, GameStopReason.PlayerGiveUp);
            break;
          }
        if (users.Count == 0) CloseRoom();
      }
    }
    void IHost.Input(int userId, ActionInput action)
    {
      if (State == RoomState.Battling)
        if (game.GetPlayer(userId) != null && game.InputAction(userId, action))
        {
          timer.Pause(userId);
          game.TryContinue();
        }
        else InformGameStop(userId, GameStopReason.InvalidInput);
    }

    internal event Action<UserInformation, int[]> SendInformation;
    void OnSendInformation(UserInformation info, params int[] userIds)
    {
      if (userIds.Length == 0) SendInformation(info, users.ToArray());
      else SendInformation(info, userIds);
    }

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

    void InformPlayerInfo(int playerId, int teamIndex, IPokemonData[] pokemons)
    {
      OnSendInformation(new PlayerInfo(teamIndex, pokemons), playerId);
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

    public void Dispose()
    {
      PropertyChanged = delegate { };
      SendInformation = delegate { };
      timer.Dispose();
    }
  }
}
