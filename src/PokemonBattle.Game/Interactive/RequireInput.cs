using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class RequirePmInput
  {
    internal static RequirePmInput Origin(Pokemon pm)
    {
      throw new NotImplementedException();
    }
    
    [DataMember(EmitDefaultValue = false)]
    int[] Moves;

    [DataMember(EmitDefaultValue = false)]
    int OnlyMove;

    [DataMember(EmitDefaultValue = false)]
    string Only; //choice/encore

    [DataMember(EmitDefaultValue = false)]
    string[] Block; //封印挑拨寻衅回复封印残废

    [DataMember(EmitDefaultValue = false)]
    string CantWithdraw;

    [DataMember(EmitDefaultValue = false)]
    int CW_pm; //如果因为队方特性

    [DataMember(EmitDefaultValue = false)]
    int CW_a;

    internal RequirePmInput(PokemonProxy pm)
    {
    }
  }
  
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class RequireInput
  {
    internal static RequireInput Origin()
    {
      throw new NotImplementedException();
    }

    [DataMember]
    RequirePmInput[] Pms;
  }
}
