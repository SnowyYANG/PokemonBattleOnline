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
    
    public override void Attacked(DefContext def)
    {
      var pm = def.Defender;
      pm.OnboardPokemon.RemoveCondition("Illusion");
      pm.Controller.ReportBuilder.Add(GameEvents.OutwardChange.All("DeIllusion", pm));
    }
  }
}
