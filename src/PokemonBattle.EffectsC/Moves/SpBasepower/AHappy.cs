using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Effects.Moves
{
    class AHappy : AttackMoveE 
    {
        public AHappy(int moveId)
            : base(moveId)
        {
        }

        protected override void CalculateBasePower(DefContext def)
        {
            double pw=def.Defender.Pokemon.Happiness /2.5;
            def.BasePower = (int)pw;
        }
    }

    class BHappy : AttackMoveE
    {
      public BHappy(int moveId)
        : base(moveId)
      {
      }

      protected override void CalculateBasePower(DefContext def)
      {
        double pw = (255 - def.Defender.Pokemon.Happiness) / 2.5;
        def.BasePower = (int)pw;
      }
    }
}
