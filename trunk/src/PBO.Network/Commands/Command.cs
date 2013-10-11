using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public abstract class S2C
  {
    public abstract void Execute(Client client);
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  public abstract class C2S
  {
  }
}
