using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class MoveLearnItem
  {
    [DataMember]
    public int MoveId { get; private set; }

    [DataMember]
    public MoveLearnMethod Method { get; private set; }

    [DataMember]
    public int Detail { get; private set; }
  }
}
