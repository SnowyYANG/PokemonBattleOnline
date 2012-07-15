﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Interactive
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
      info.Moves = new int[4];
      for (int i = 0; i < 4; i++)
        if (pm.Moves[i] != null) info.Moves[i] = pm.Moves[i].Type.Id;
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
