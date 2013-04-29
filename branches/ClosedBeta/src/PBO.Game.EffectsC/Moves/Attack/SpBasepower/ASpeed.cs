using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
    class ElectroBall:AttackMoveE 
    {
        public ElectroBall(int MoveId)
            : base(MoveId)
        { }

        protected override void CalculateBasePower(DefContext def)
        {
          int pwb = (int)(def.AtkContext.Attacker.Speed / def.Defender.Speed);
          if (pwb >= 4)
            def.BasePower = 150;
          else if (pwb >= 3)
            def.BasePower = 120;
          else if (pwb >= 2)
            def.BasePower = 80;
          else
            def.BasePower = 60;
        }
    }

    class GyroBall : AttackMoveE
    {
      public GyroBall(int MoveId)
        : base(MoveId)
      { }
      
      protected override void CalculateBasePower(DefContext def)
      {
        int pwb = (int)(25 * def.Defender.Speed / def.AtkContext.Attacker.Speed);
        if (pwb > 150) def.BasePower = 150;
        else if (pwb < 1) def.BasePower = 1;
        else def.BasePower = pwb;
      }
    }
}
