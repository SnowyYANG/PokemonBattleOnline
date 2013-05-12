using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using User = LightStudio.Tactic.Messaging.User<LightStudio.PokemonBattle.Messaging.UserExtension>;

namespace LightStudio.PokemonBattle.Messaging
{
  public abstract class ClientService : Tactic.Messaging.ClientService<UserExtension>
  {
    public ClientService(Client client, params byte[] receiveMessageHeaders)
      : base(client, receiveMessageHeaders)
    {
    }

    protected internal new Client Client
    { get { return (Client)base.Client; } }

    protected void SendMessage(byte header, IMessagable obj, params int[] receivers)
    {
      var message = obj.ToMessage();
      SendMessage(header, writer =>
      {
        writer.Write(message.Header);
        writer.Write(message.Content);
      }, receivers);
    }
  }
}
