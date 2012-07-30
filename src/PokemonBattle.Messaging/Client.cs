using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LightStudio.Tactic.Messaging;

namespace LightStudio.PokemonBattle.Messaging
{
  public sealed class Client : Client<RoomInfo>
  {
    public static Client NewTcpClient(IPAddress serverAddress, int serverPort)
    {
      return new Client(Factory.TcpMessageClient(serverAddress, serverPort));
    }

    private Client(Tactic.Messaging.Primitive.IMessageClient client)
      : base(client)
    {
    }

    /// <summary>
    /// currently simply do nothing
    /// </summary>
    internal void RegisterRoom()
    {
    }

    internal void RegisterRoomUser(int userId)
    {
      Send(new TextMessage("u", userId.ToString()));
    }
  }
}
