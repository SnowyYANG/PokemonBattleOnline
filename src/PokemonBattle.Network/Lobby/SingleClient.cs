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
  internal class SingleClient : DisposableObject, IBattleClient
  {
    event Action<IUserController> IBattleClient.EnterSucceed
    {
      add { user.EnterSucceed += value; }
      remove { user.EnterSucceed -= value; }
    }
    private readonly Player user;

    public SingleClient(int hostId)
    {
      HostId = hostId;
      user = new Player(hostId);
      user.SendCommand += (command) => OnMessageSent(command.ToMessage(), HostId);
    }

    public int HostId
    { get; private set; }

    public void Start(PokemonCustomInfo[] pokemons)
    {
      if (user.Role == UserRole.Player)
      {
        user.JoinGame(pokemons, 1);
      }
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
      if (info != null) ((IUser)user).ExecuteInformation(info);
    }

    public event EventHandler<MessageSentEventArgs> MessageSent = delegate { };
    private void OnMessageSent(IMessage message, params int[] receivers)
    {
      MessageSent(this, new MessageSentEventArgs(receivers, message));
    }

    #endregion

  }
}
