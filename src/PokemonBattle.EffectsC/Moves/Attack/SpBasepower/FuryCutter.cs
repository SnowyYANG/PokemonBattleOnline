using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class FuryCutter : AttackMoveE
  {
    public FuryCutter(int id)
      : base(id)
    {
    }
    protected override void CalculateBasePower(DefContext def)
    {
      var c = def.AtkContext.Attacker.OnboardPokemon.GetCondition("LastMove");
      if (c != null && c.Move == Move) def.BasePower = 20 * (1 << (c.Int > 3 ? 3 : c.Int));
      else def.BasePower = 20;
    }
  }
}
