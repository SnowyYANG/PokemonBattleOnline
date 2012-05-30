using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using LightStudio.Tactic.Messaging.Lobby;
using LightStudio.PokemonBattle.Room;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Messaging
{
  internal class SingleHost : DisposableObject, IBattleClient, IBattleHost
  {
    event Action<IUserController> IBattleClient.EnterSucceed
    {
      add { user.EnterSucceed += value; }
      remove { user.EnterSucceed -= value; }
    }
    private Host host;
    private Player user;

    public SingleHost(int adminId, GameInitSettings settings)
    {
      host = new Host(settings);
      host.SendInformation += (info, ids) => OnMessageSent(info.ToMessage(), ids);
      host.PropertyChanged += (sender, e) =>
        {
          if (e.PropertyName == Host.CAN_START_GAME) ((Host)sender).StartGame();
        };
      user = new Player(adminId);
      user.SendCommand += (command) => OnMessageSent(command.ToMessage(), adminId);
      user.Quited += () => host.CloseRoom();
    }

    int IBattleClient.HostId
    { get { return LobbyService.User.Id; } }

    public IUserController UserController
    { get { return user; } }
    public Host AdminController
    { get { return host; } }

    public void Start(PokemonCustomInfo[] pokemons)
    {
      user.JoinGame(pokemons, 0);
    }

    public void Start()
    {
      //useless, for host_PropertyChanged has already done this
      host.StartGame();
    }

    protected override void DisposeManagedResources()
    {
      base.DisposeManagedResources();
      host.Dispose();
      user.Dispose();
    }

    #region Handle Message

    public void OnReceived(int senderId, IMessage message)
    {
      IMessagable obj = message.GetMessageObjectOrNull();
      if (obj != null)
        if (obj is IHostCommand) ((IHost)host).ExecuteCommand((IHostCommand)obj, senderId);
        else if (obj is IUserInformation) ((IUser)user).ExecuteInformation((IUserInformation)obj);
    }
    public void OnReceived(IMessage message)
    {
      IUserInformation info = message.GetMessageObjectOrNull() as IUserInformation;
      if (info != null) ((IUser)user).ExecuteInformation(info);
    }

    public event EventHandler<MessageSentEventArgs> MessageSent;
    private void OnMessageSent(IMessage message, params int[] receivers)
    {
      if (receivers.Length > 0)
        MessageSent(this, new MessageSentEventArgs(receivers, message));
    }

    #endregion
  }
}
