using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game.Host.Sp;
using As = PokemonBattleOnline.Game.Host.Sp.Abilities;

namespace PokemonBattleOnline.Game.Host.Effects.Triggers
{
  class IsGroundAffectable : IIsGroundAffectable
  {
    public bool Execute(PokemonProxy pm, bool abilityAvailable, bool raiseAbility, bool ground)
    {
      var o = pm.OnboardPokemon;
      return
        (o.HasCondition("SmackDown") || o.HasCondition("Ingrain") || ground && pm.Controller.Board.HasCondition("Gravity")) || pm.Item.IronBall() ||
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
