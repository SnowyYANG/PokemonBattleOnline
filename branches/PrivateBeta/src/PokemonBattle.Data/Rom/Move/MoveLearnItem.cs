using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class MoveLearnItem
  {
#if DEBUG
    public MoveLearnItem(int move)
    {
      MoveId = move;
    }
#endif

    [DataMember(EmitDefaultValue = false)]
    public int? Form { get; private set; }

    [DataMember]
    public int MoveId { get; private set; }

    [DataMember]
    public MoveLearnMethod Method { get; private set; }

    [DataMember]
    public int Gen { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public int Detail { get; private set; }
  }
}
