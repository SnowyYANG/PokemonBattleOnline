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
  internal abstract class RoomUserBase : IRoomUser, IRoom, IDisposable
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
    private ObservableCollection<int> players;
    private ObservableCollection<int> spectators;
    private GameOutward game;
    private Dispatcher gameEventsDispatcher;

    protected RoomUserBase(int hostId)
    {
      HostId = hostId;
      gameEventsDispatcher = new Dispatcher("RoomUserBase", true);
    }

    public ReadOnlyObservableCollection<int> Spectators { get; private set; }
    public ReadOnlyObservableCollection<int> Players { get; private set; }
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
      players.Add(userId);
    }
    void IRoomInformer.InformUserQuit(int userId)
    {
      if (!spectators.Remove(userId)) players.Remove(userId);
    }
    void IRoomInformer.InformUserKicked(int userId)
    {
      if (!spectators.Remove(userId)) players.Remove(userId);
    }
    void IRoomInformer.InformEnterFailed(string message)
    {
      EnterFailed(message);
    }
    void IRoomInformer.InformEnterSucceed(GameInitSettings settings, int[] players, int[] spectators)
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

    protected virtual void InformEnterSucceed(GameInitSettings settings, int[] players, int[] spectators)
    {
      this.players = new ObservableCollection<int>(players);
      this.spectators = new ObservableCollection<int>(spectators);
      Players = new ReadOnlyObservableCollection<int>(this.players);
      Spectators = new ReadOnlyObservableCollection<int>(this.spectators);
      game = new GameOutward(settings);
    }
    protected virtual void InformReportUpdate(ReportFragment fragment)
    {
      if (RoomState != RoomState.GameStarted) RoomState = RoomState.GameStarted;
      game.Update(fragment);
    }
    void IGameInformer.InformReportUpdate(ReportFragment fragment)
    {
      gameEventsDispatcher.BeginInvoke((Action<ReportFragment>)InformReportUpdate, fragment);
    }

    #region Player Only
    protected abstract void InformReportAddition(PokemonAdditionalInfo pminfo);
    void IGameInformer.InformReportAddition(PokemonAdditionalInfo pminfo)
    {
      InformReportAddition(pminfo);
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
    protected Action<IHostCommand> sendCommand = delegate { };
    public event Action<IHostCommand> SendCommand
    {
      add { lock(sendCommand) sendCommand += value; }
      remove { lock(sendCommand) sendCommand -= value; }
    }
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
