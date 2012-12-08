using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class PokemonLearnList
  {
    [DataMember]
    public SortedDictionary<int, int> Lvs;
    [DataMember]
    public HashSet<int> TM;
    [DataMember]
    public HashSet<int> HM; //非第五代可空
    [DataMember]
    public HashSet<int> Tutor; //区分教学和技能机是为了显示
    [DataMember]
    public HashSet<int> Breed;
    [DataMember]
    public HashSet<int> Sp; //仅第三代用 XD
  }
}
