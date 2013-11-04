using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class MoveLv7DChange
  {
    [DataMember]
    public StatType Type { get; private set; }
    [DataMember]
    public sbyte Change { get; private set; }
    [DataMember]
    public byte Probability { get; private set; }
  }
}
