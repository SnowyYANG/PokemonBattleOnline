using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Attack5Turns : AttackMoveE
  {
    public Attack5Turns(int id)
      : base(id)
    {
    }

    public override AtkContext BuildAtkContext(PokemonProxy pm)
    {
      var atk = base.BuildAtkContext(pm);
      atk.SetCondition("MultiTurn", 5);
      return atk;
    }
    protected override void CalculateBasePower(DefContext d)
    {
      d.BasePower = 30 * (1 << ((d.AtkContext.Attacker.OnboardPokemon.HasCondition("DefenseCurl") ? 6 : 5) - d.AtkContext.GetCondition("MultiTurn").Turn));
    }
  }
}
