using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Abilities
{
  /// <summary>
  /// Also SpCondition
  /// </summary>
  class Illusion : AbilityE
  {
    public Illusion(int id)
      : base(id)
    {
    }

    private void DeIllusion(PokemonProxy pm)
    {
      pm.OnboardPokemon.RemoveCondition("Illusion");
      pm.Controller.ReportBuilder.Add(GameEvents.OutwardChange.All("DeIllusion", pm));
    }

    public override void Detach(PokemonProxy pm)
    {
      DeIllusion(pm);
    }
    public override void Attacked(DefContext def)
    {
      DeIllusion(def.Defender);
    }
  }
}
