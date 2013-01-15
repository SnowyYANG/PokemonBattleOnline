using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Abilities
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
      if (pm.OnboardPokemon.RemoveCondition("Illusion")) pm.Controller.ReportBuilder.Add(GameEvents.OutwardChange.DeIllusion("DeIllusion", pm));
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
