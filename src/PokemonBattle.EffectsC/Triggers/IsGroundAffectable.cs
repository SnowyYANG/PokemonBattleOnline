using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;
using As = LightStudio.PokemonBattle.Game.Host.Sp.Abilities;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Triggers
{
  class IsGroundAffectable : IIsGroundAffectable
  {
    public bool Execute(PokemonProxy pm, bool abilityAvailable, bool raiseAbility)
    {
      var o = pm.OnboardPokemon;
      return
        (o.HasCondition("SmackDown") || o.HasCondition("Ingrain") || pm.Controller.Board.HasCondition("Gravity")) || pm.Item.IronBall() ||
        !
        (
          o.HasType(BattleType.Flying) ||
          o.HasCondition("MagnetRise") || o.HasCondition("Telekinesis") ||
          pm.Item.AirBalloon() ||
          (abilityAvailable && (raiseAbility ? pm.RaiseAbility(As.LEVITATE) : pm.Ability.Id == As.LEVITATE))
        );
    }
  }
}
