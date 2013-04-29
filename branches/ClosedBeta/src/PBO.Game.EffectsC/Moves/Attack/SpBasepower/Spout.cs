using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
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
