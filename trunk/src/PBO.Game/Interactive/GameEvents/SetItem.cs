﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class SetItem : GameEvent
  {
    [DataMember]
    public int Pm;
    [DataMember(EmitDefaultValue = false)]
    public int Item;

    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null) pm.Item = Item == 0 ? null : RomData.GetItem(Item);
    }
  }
}
