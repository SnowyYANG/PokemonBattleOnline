using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;


namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
    class GrassKnot : AttackMoveE
    {
        public GrassKnot(int moveId)
            :base(moveId)
        {
        }

        protected override void CalculateBasePower(DefContext def)
        {
            double w = def.Defender.Weight;
            if (w >= 200)
                def.BasePower = 120;
            else if (w >= 100)
                def.BasePower = 100;
            else if (w >= 50)
                def.BasePower = 80;
            else if (w >= 25)
                def.BasePower = 60;
            else if (w >= 10)
                def.BasePower = 40;
            else
                def.BasePower = 20;
        }
    }
}
