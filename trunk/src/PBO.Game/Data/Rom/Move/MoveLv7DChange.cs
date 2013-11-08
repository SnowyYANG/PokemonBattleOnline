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
    public int Change { get; private set; }
    [DataMember]
    public int Probability { get; private set; }
  }
}
