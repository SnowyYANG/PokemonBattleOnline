using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightStudio.Tactic.Logging;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Room
{
  public enum UserRole : byte
  {
    Player,
    Spectator
  }
  
  internal abstract class User : IUser, IUserController
  {
    protected readonly int HostId;

    public event Action<string> EnterFailed = delegate { };
    public event Action<IUserController> EnterSucceed;
    public event Action Quited = delegate { };
    public event Action GameEnd = delegate { };
    public event Action<string> Error
    {
      add { error += value; }
      remove { error -= value; }
    }

    protected Action<string> error = delegate { };
    private ObservableCollection<int> players;
    private ObservableCollection<int> spectators;
    private GameOutward game;
    
    protected User(int hostId)
    {
      HostId = hostId;
    }

    public ReadOnlyObservableCollection<int> Spectators { get; private set; }
    public ReadOnlyObservableCollection<int> Players { get; private set; }
    public abstract UserRole Role { get; }
    public virtual IPlayerController PlayerController
    { get { return null; } }
    public GameOutward Game
    { get { return game; } }
    public RoomState RoomState
    { get; private set; }
    
    #region Information
    void IUser.ExecuteInformation(IUserInformation info)
    {
      info.Execute(this);
    }
    void IRoomInformer.InformChat(int userId, MessageTarget target, string content)
    {
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
    void IRoomInformer.InformEnterSucceed(Game.GameSettings settings, int[] players, int[] spectators, int[] ids)
    {
      this.players = new ObservableCollection<int>(players);
      this.spectators = new ObservableCollection<int>(spectators);
      Players = new ReadOnlyObservableCollection<int>(this.players);
      Spectators = new ReadOnlyObservableCollection<int>(this.spectators);
      game = new GameOutward(settings);
      if (PlayerController != null)
      {
        PlayerController.SetSimulator(ids);
      }
      EnterSucceed(this);
    }
    void IGameInformer.InformTimeUp(IList<int> breakers)
    {
      RoomState = RoomState.GameEnd;
#warning unfinished
      GameEnd();
    }
    void IGameInformer.InformTimeElapsed(int remainingTime)
    {
    }
    void IGameInformer.InformGameResult(int[] gameResult, bool isStoped)
    {
      RoomState = RoomState.GameEnd;
#warning unfinished
      GameEnd();
    }

    protected virtual void InformTurn(Turn turn)
    {
      if (RoomState != RoomState.GameStarted) RoomState = RoomState.GameStarted;
      game.Update(turn);
    }
    void IGameInformer.InformTurn(Turn turn)
    {
      InformTurn(turn);
    }


    #region Player Only
    protected abstract void InformPmAdditional(PokemonAdditionalInfo pminfo);
    void IGameInformer.InformPmAdditional(PokemonAdditionalInfo pminfo)
    {
      InformPmAdditional(pminfo);
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
    protected abstract void InformRequireInput();
    void IGameInformer.InformRequireInput()
    {
      InformRequireInput();
    }
    protected abstract void InformInputFail();
    void IGameInformer.InformInputFail()
    {
      InformInputFail();
    }
    protected abstract void InformInputSucceed();
    void IGameInformer.InformInputSucceed()
    {
      InformInputSucceed();
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
    public void Chat(string content, MessageTarget target = MessageTarget.All, int targetId = 0)
    {
      sendCommand(new ChatCommand(content, target, targetId));
    }
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
    }
  }
}
