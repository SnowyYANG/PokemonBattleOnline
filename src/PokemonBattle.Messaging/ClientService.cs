using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using IUser = LightStudio.Tactic.Messaging.IUser<LightStudio.PokemonBattle.Messaging.RoomInfo>;

namespace LightStudio.PokemonBattle.Messaging
{
  public abstract class ClientService : Tactic.Messaging.ClientService<RoomInfo>
  {
    public ClientService(Client client, params byte[] receiveMessageHeaders)
      : base(client, receiveMessageHeaders)
    {
    }

    protected internal new Client Client
    { get { return (Client)base.Client; } }
  }
}
