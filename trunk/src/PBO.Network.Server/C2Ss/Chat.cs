using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Network.Commands;

namespace PokemonBattleOnline.Network.C2Ss
{
  [DataContract(Namespace = PBOMarks.JSON)]
  internal class ChatC2S : Commands.ChatC2S, IC2S
  {
    public void Execute(ServerUser user)
    {
      switch (Mode)
      {
        case ChatMode.Public:
          user.Server.SendAll(new ChatS2C(Mode, user.User.Id, Chat));
          break;
        case ChatMode.Room:
          throw new NotImplementedException();
          break;
        case ChatMode.Private:
          throw new NotImplementedException();
          break;
      }
    }
  }
}
