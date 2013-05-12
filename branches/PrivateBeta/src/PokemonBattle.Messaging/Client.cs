using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LightStudio.Tactic.Messaging;

namespace LightStudio.PokemonBattle.Messaging
{
  public sealed class Client : Client<UserExtension>
  {
    public static Client NewTcpClient(IPAddress serverAddress, int serverPort)
    {
      return new Client(Factory.TcpMessageClient(serverAddress, serverPort));
    }

    private Client(Tactic.Messaging.Primitive.IMessageClient client)
      : base(client)
    {
    }

    protected override void OnReceive(IMessage message)
    {
      if (message.Header == "u") message.Resolve((reader) =>
        {
          int userId = reader.ReadInt16();
          var user = GetUser(userId);
          if (user != null) user.Extension.LastRoomId = reader.ReadInt16();
        });
      else base.OnReceive(message);
    }

    /// <summary>
    /// currently simply do nothing
    /// </summary>
    internal void RegisterRoom()
    {
    }
    internal void DeregisterRoom()
    {
    }

    internal void RegisterRoomUser(int userId)
    {
      Send(new TextMessage("u", userId.ToString()));
    }
  }
}
