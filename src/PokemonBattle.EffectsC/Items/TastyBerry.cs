using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Items
{
  class TastyBerry : ItemE
  {
    public TastyBerry(int id)
      : base(id)
    {
    }

    private void E(PokemonProxy pm)
    {
      if (Sp.Abilities.Gluttony(pm) && pm.CanHpRecover(false))
      {
        pm.HpRecoverByOneNth(8, false, "ItemRecover", Item.Id, true);
        if (pm.Pokemon.Nature.DislikeTaste(Sp.Items.GetTaste(Item))) pm.AddState(pm, AttachedState.Confuse, false);
      }
    }
    public override void Attach(PokemonProxy pm)
    {
      E(pm);
    }
    public override void HpChanged(PokemonProxy pm)
    {
      E(pm);
    }
  }
}
