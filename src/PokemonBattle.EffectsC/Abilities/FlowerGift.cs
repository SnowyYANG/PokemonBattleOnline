using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Abilities
{
  class FlowerGift : AbilityE
  {
    public FlowerGift(int id)
      : base(id)
    {
    }
    public override void Detach(PokemonProxy pm)
    {
      if (pm.CanChangeForm(421, 0)) pm.ChangeForm(0);
    }
  }
}
