using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightStudio.Tactic.Logging;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  internal abstract class RoomUserClient : IRoomUser, IRoom, IDisposable
  {
    public readonly int HostId;

    public event Action<string> EnterFailed = delegate { };
    public event Action EnterSucceed;
    public event Action GameEnd = delegate { };
    public event Action Quited = delegate { };
    public event Action<string> Error
    {
      add { error += value; }
      remove { error -= value; }
    }

    protected Action<string> error = delegate { };
    private ObservableCollection<Player> players;
    private ObservableCollection<int> spectators;
    private GameOutward game;
    private Dispatcher gameEventsDispatcher;

    protected RoomUserClient(int hostId)
    {
      HostId = hostId;
      gameEventsDispatcher = new Dispatcher("RoomUserClient", true);
    }

    public ReadOnlyObservableCollection<Player> Players { get; private set; }
    public ReadOnlyObservableCollection<int> Spectators { get; private set; }
    public GameInitSettings Settings { get; private set; }
    public GameOutward Game
    { get { return game; } }
    public RoomState RoomState { get; private set; }
    public abstract Tactic.Messaging.UserState State { get; }
    public abstract IPlayerController PlayerController { get; }
    
    #region Information
    void IRoomUser.ExecuteInformation(IUserInformation info)
    {
      UIDispatcher.Invoke((Action<IRoomUser>)(info.Execute), this);
    }
    void IRoomInformer.InformUserSpectateGame(int userId)
    {
      spectators.Add(userId);
    }
    void IRoomInformer.InformUserJoinGame(int userId, int teamId)
    {
      players.Add(new Player(userId, teamId));
    }
    private void RemoveUser(int userId)
    {
      if (!spectators.Remove(userId))
      {
        foreach(Player p in players)
          if (p.Id == userId)
          {
            players.Remove(p);
            break;
          }
      }
    }
    void IRoomInformer.InformUserQuit(int userId)
    {
      RemoveUser(userId);
    }
    void IRoomInformer.InformUserKicked(int userId)
    {
      RemoveUser(userId);
    }
    void IRoomInformer.InformEnterFailed(string message)
    {
      EnterFailed(message);
    }
    void IRoomInformer.InformEnterSucceed(GameInitSettings settings, Player[] players, int[] spectators)
    {
      InformEnterSucceed(settings, players, spectators);
      EnterSucceed();
    }
    void IGameInformer.InformPlayerInputed(int player)
    {
      throw new NotImplementedException();
    }
    void IGameInformer.InformGameResult(int team0, int team1)
    {
      RoomState = RoomState.GameEnd;
#warning unfinished
      GameEnd();
    }
    void IGameInformer.InformGameStop(GameStopReason reason, int player)
    {
      throw new NotImplementedException();
    }
    void IGameInformer.InformGameTie()
    {
      throw new NotImplementedException();
    }
    void IGameInformer.InformTimeUp(int[] remainingTime)
    {
      throw new NotImplementedException();
    }

    protected virtual void InformEnterSucceed(GameInitSettings settings, Player[] players, int[] spectators)
    {
      Settings = settings;
      this.players = new ObservableCollection<Player>(players);
      this.spectators = new ObservableCollection<int>(spectators);
      Players = new ReadOnlyObservableCollection<Player>(this.players);
      Spectators = new ReadOnlyObservableCollection<int>(this.spectators);
    }
    protected virtual void InformReportUpdate(ReportFragment fragment)
    {
      if (RoomState != RoomState.GameStarted)
      {
        RoomState = RoomState.GameStarted;
        Dictionary<int, string> ps = new Dictionary<int,string>();
        foreach(Player p in players) ps.Add(p.Id, p.GetName());
        game = new GameOutward(Settings, ps);
      }
      game.Update(fragment);
    }
    void IGameInformer.InformReportUpdate(ReportFragment fragment)
    {
      gameEventsDispatcher.BeginInvoke((Action<ReportFragment>)InformReportUpdate, fragment);
    }

    #region Player Only
    protected abstract void InformRequireInput(RequireInput pminfo);
    void IGameInformer.InformRequireInput(RequireInput pminfo)
    {
      InformRequireInput(pminfo);
    }
    protected abstract void InformRequestTie();
    void IGameInformer.InformRequestTie()
    {
      InformRequestTie();
    }
    protected abstract void InformTieRejected();
    void IGameInformer.InformTieRejected()
    {
      InformTieRejected();
    }
    #endregion
    #endregion

    #region Command
    protected Action<IHostCommand> sendCommand;
    public event Action<IHostCommand> SendCommand
    { add { sendCommand += value; } remove { } }
    public abstract void EnterRoom();
    public void Quit()
    {
      sendCommand(new QuitCommand());
      Quited();
    }
    #endregion

    public void Dispose()
    {
      Quit();
      EnterFailed = delegate { };
      EnterSucceed = delegate { };
      GameEnd = delegate { };
      Quited = delegate { };
      error = delegate { };
      gameEventsDispatcher.Dispose();
    }
  }
}
