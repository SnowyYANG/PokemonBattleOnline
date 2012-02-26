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
  internal class SpectatorClient : DisposableObject, IBattleClient
  {
    event Action<IUserController> IBattleClient.EnterSucceed
    {
      add { user.EnterSucceed += value; }
      remove { user.EnterSucceed -= value; }
    }
    private readonly Room.User user;

    public SpectatorClient(int hostId)
    {
      HostId = hostId;
      user = new Spectator(hostId);
      user.SendCommand += (command) => OnMessageSent(command.ToMessage(), HostId);
    }

    public int HostId
    { get; private set; }

    public Room.IUserController UserController
    { get { return user; } }

    public void Start(PokemonCustomInfo[] pokemons)
    {
    }

    protected override void DisposeManagedResources()
    {
      base.DisposeManagedResources();
      user.Dispose();
    }

    #region Handle Message

    public void OnReceived(IMessage message)
    {
      IUserInformation info = message.GetMessageObjectOrNull() as IUserInformation;
      if (info != null) UIDispatcher.Invoke((Action<IUserInformation>)((IUser)user).ExecuteInformation, info);
    }

    public event EventHandler<MessageSentEventArgs> MessageSent = delegate { };
    private void OnMessageSent(IMessage message, params int[] receivers)
    {
      MessageSent(this, new MessageSentEventArgs(receivers, message));
    }

    #endregion
  }
}
