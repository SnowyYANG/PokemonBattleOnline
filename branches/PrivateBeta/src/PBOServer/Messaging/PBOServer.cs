﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using LightStudio.Tactic.Messaging.Primitive;
using User = LightStudio.Tactic.Messaging.User<LightStudio.PokemonBattle.Messaging.RoomInfo>;

namespace LightStudio.PokemonBattle.Messaging
{
  internal class PBOServer : Server<RoomInfo>
  {
    public static PBOServer NewTcpServer(int port)
    {
      return new PBOServer(Factory.TcpMessageServer(port));
    }
    
    private PBOServer(IMessageServer server)
      : base(server)
    {
    }

    protected override void OnReceive(int userId, IMessage message)
    {
      if (message.Header == "u") OnRoomUserRegistered(Convert.ToInt16(message.Content), userId);
      else base.OnReceive(userId, message);
    }

    public void OnRoomRegistered()
    {
      //simply do nothing
    }
    public void OnRoomUserRegistered(short hostId, int user)
    {
      var u = GetUser(user);
      if (u != null) u.Extension.LastRoomId = hostId;
      Broadcast(new TextMessage("u", (writer) =>
        {
          writer.Write((short)user);
          writer.Write(hostId);
        }));
    }
  }
}
