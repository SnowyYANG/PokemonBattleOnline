using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;


namespace LightStudio.PokemonBattle.Effects.Moves
{
    class GrassKnot : AttackMoveE
    {
        public GrassKnot(MoveType move)
            :base(move)
        {
        }

        protected override void CalculatePower(AtkContext atk)
        {
            double w = atk.Target.Defender.Pokemon.PokemonType.Weight;
            if (w >= 200)
                atk.Power = 120;
            else if (w >= 100)
                atk.Power = 100;
            else if (w >= 50)
                atk.Power = 80;
            else if (w >= 25)
                atk.Power = 60;
            else if (w >= 10)
                atk.Power = 40;
            else
                atk.Power = 20;
        }
    }
}
