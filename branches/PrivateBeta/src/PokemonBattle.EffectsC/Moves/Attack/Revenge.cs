using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Revenge : AttackMoveE
  {
    public Revenge(int id)
      : base(id)
    {
    }
    protected override void CalculateBasePower(DefContext def)
    {
      var o = def.AtkContext.Attacker.OnboardPokemon.GetCondition("Damage");
      if (o != null && o.By == def.Defender) def.BasePower = 120;
      else def.BasePower = 60;
    }
  }
}
