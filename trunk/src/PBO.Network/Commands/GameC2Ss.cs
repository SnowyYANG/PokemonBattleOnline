using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network.Commands
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class PrepareC2S : IC2S
  {
    [DataMember]
    PokemonData[] a_;

    public PrepareC2S(IPokemonData[] pms)
    {
      a_ = pms.Select((p) => (PokemonData)p).ToArray();
    }
    protected PrepareC2S()
    {
    }
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  public class InputC2S : ActionInput, IC2S
  {
    public InputC2S(ActionInput action)
      : base(action)
    {
    }
    protected InputC2S()
      : base()
    {
    }
  }
}
