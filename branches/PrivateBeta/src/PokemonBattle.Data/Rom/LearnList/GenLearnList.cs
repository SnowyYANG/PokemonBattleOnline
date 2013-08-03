using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class GenLearnList : SimpleData
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
