using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network.C2SEs
{
  [DataContract(Namespace = PBOMarks.JSON)]
  internal class PrepareC2S : Commands.PrepareC2S, IC2SE
  {
    [DataMember]
    PokemonData[] a_;

    public void Execute(ServerUser su)
    {
      var room = su.Room;
      if (room != null) room.Prepare(su, a_);
    }
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  internal class InputC2S : Commands.InputC2S, IC2SE
  {
    private InputC2S()
      : base()
    {
    }

    public void Execute(ServerUser su)
    {
      var room = su.Room;
      if (room != null) room.Input(su, this);
    }
  }
}
