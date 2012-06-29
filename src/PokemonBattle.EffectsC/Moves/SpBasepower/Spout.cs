using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Effects.Moves
{
  class Spout:AttackMoveE
  {
    public Spout(int MoveId)
      : base(MoveId)
    { }

    protected override void CalculateBasePower(DefContext def)
    {
      int pwb = (int)(150 * def.AtkContext.Attacker.Hp / def.AtkContext.Attacker.Pokemon.Hp.Origin);

      if(pwb==0)
        def.BasePower=1;
      else
        def.BasePower=pwb;
    }
  }
}
