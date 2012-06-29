using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Effects.Abilities
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
      def.Defender.OnboardPokemon.RemoveCondition("Illusion");
      def.Defender.Controller.ReportBuilder.Add(new Interactive.GameEvents.DeIllusion(def.Defender));
    }
  }
}
