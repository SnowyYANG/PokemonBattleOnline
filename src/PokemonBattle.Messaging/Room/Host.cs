using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public event Action Closed = delegate { };
    internal readonly GameInitSettings GameSettings;
    private readonly Dispatcher dispatcher;
    private readonly HashSet<int> users;
    private readonly HashSet<int> playerIds;
    private readonly ObservableCollection<int> spectators;
    private readonly IGame game;
    private readonly bool autoStart;
    
    public Host(GameInitSettings settings, bool autoStart)
    {
      dispatcher = new Dispatcher("Host", true);
      users = new HashSet<int>();
      playerIds = new HashSet<int>();
      
      //players = new ObservableCollection<int>();
      //Players = new ReadOnlyObservableCollection<int>(players);
      spectators = new ObservableCollection<int>();
      Spectators = new ReadOnlyObservableCollection<int>(spectators);
      
      game = GameFactory.CreateGame(settings, settings.NextId);
      game.ReportUpdated += InformReportUpdate;
      GameSettings = settings;
      this.autoStart = autoStart;
    }

    public ReadOnlyObservableCollection<int> Spectators
    { get; private set; }
    public ReadOnlyObservableCollection<int> Players
    { get; private set; }
    public ReadOnlyObservableCollection<int> RemainingTime
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
        playerIds.Add(userId);
        users.Add(userId);
        InformEnterSucceed(userId, true);
        if (CanStartGame != canStartGame) OnPropertyChanged(CAN_START_GAME);
        if (CanStartGame && autoStart) StartGame();
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
        if (playerIds.Remove(userId))
        {
          if (State == RoomState.GameStarted)
            InformGameStop(userId, GameStopReason.UserQuit);
        }
        else
        {
          spectators.Remove(userId);
        }
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
        if (game.InputAction(userId, action))
        {
          InformPlayerInputed(userId);
          game.TryContinue();
        }
        else
        {
          InformGameStop(userId, GameStopReason.InvalidInput);
        }
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
        OnSendInformation(EnterSucceedInfo.Player(this, ids), userId);
      }
      else
      {
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
      throw new NotImplementedException();
      //State = RoomState.GameEnd;
      //OnSendInformation(GameEndInfo.TimeUp(remainingTime));
    }

    void InformReportUpdate(ReportFragment fragment)
    {
      OnSendInformation(new ReportUpdateInfo(fragment));
    }
    void InformReportAddition(PokemonAdditionalInfo info)
    {
      OnSendInformation(new ReportAdditionInfo(info), info.GetReceiversId());
    }
    void InformPlayerInputed(int userId)
    {
      OnSendInformation(new PlayerInputedInfo(userId));
    }

    void InformRequestTie()
    {
    }
    void InformTieRejected()
    {
    }
    #endregion
    #endregion

    public void Kick(int targetId)
    {
    }
    public void StartGame()
    {
      if (game.Start()) State = RoomState.GameStarted;
    }
    public void CloseRoom()
    {
      Closed();
    }

    #region PropertyChanged
    private void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      PropertyChanged(this, e);
    }
    #endregion

    public void Dispose()
    {
      PropertyChanged = delegate { };
      SendInformation = delegate { };
      dispatcher.Dispose();
    }
  }
}
