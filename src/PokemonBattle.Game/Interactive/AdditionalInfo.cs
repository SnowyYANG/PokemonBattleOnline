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
  public class PokemonAdditionalInfo
  {
    internal static PokemonAdditionalInfo RivalAbilityNotify(PokemonProxy pm)
    {
      PokemonAdditionalInfo info = new PokemonAdditionalInfo();
      info.Id = pm.Id;
      info.Ability = pm.Ability.Id;
      info.receiversId = new int[0];
      return info;
    }
    internal static PokemonAdditionalInfo OwnerMovesNotify(PokemonProxy pm)
    {
      PokemonAdditionalInfo info = new PokemonAdditionalInfo();
      info.Id = pm.Id;
      info.Moves = (from m in pm.Moves select m.Type.Id).ToArray();
      info.receiversId = new int[] { pm.Pokemon.Owner.Id };
      return info;
    }
    
    [DataMember]
    int Id;
    [DataMember(EmitDefaultValue = false)]
    int[] Moves;
    [DataMember(EmitDefaultValue = false)]
    int Ability;

    int[] receiversId;
    public int[] GetReceiversId()
    {
      return receiversId;
    }
  }
}
