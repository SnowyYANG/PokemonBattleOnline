using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Network.Commands;

namespace PokemonBattleOnline.Network.C2SEs
{
  [DataContract(Name = "chat", Namespace = PBOMarks.JSON)]
  internal class ChatC2S : Commands.ChatC2S, IC2SE
  {
    public void Execute(PboUser su)
    {
      var name = su.User.Name;
      var server = su.Server;
      var s2c = new ChatS2C(Mode, name, Chat);
      switch (Mode)
      {
        case ChatMode.Public:
          break;
        case ChatMode.Room:
          var room = su.Room;
          if (room != null) su.Server.Send(room, s2c);
          else Console.WriteLine("room is null for user " + name);
          break;
        case ChatMode.Private:
          break;
      }
    }
  }
}
