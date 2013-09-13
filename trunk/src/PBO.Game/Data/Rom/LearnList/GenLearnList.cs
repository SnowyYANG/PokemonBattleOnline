using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class GenLearnList
  {
    [DataMember]
    public int Gen;
    [DataMember]
    public PokemonLearnList[] Raw;

    public PokemonLearnList GetLearnList(int number)
    {
      return Raw.ValueOrDefault(number - 1);
    }
  }
}
