using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class EchoedVoice : AttackMoveE
  {
    public EchoedVoice(int id)
      : base(id)
    {
    }
    
    protected override void CalculateBasePower(DefContext def)
    {
      var c = def.AtkContext.Attacker.OnboardPokemon.GetCondition("LastMove");
      if (c != null && c.Move == Move)
      {
        def.BasePower = 40 * (c.Int + 1);
        if (def.BasePower > 200) def.BasePower = 200;
      }
      else def.BasePower = 40;
    }
  }
}
