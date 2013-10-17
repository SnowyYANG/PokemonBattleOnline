using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.C2Ss
{
  [DataContract(Namespace = PBOMarks.JSON)]
  internal class SetSeatC2S : Commands.SetSeatC2S, IC2S
  {
    public void Execute(ServerUser user)
    {
    }
  }
}
