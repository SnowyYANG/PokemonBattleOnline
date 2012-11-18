using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Abilities
{
  class ZenMode : AbilityE
  {
    public ZenMode(int id)
      : base(id)
    {
    }
    public override void Detach(PokemonProxy pm)
    {
      if (pm.CanChangeForm(555, 0)) pm.ChangeForm(0);
    }
  }
}
