using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;


namespace LightStudio.PokemonBattle.Effects.Moves
{   
    class SpRangeMove : AttackMoveE
    {
      CoordY y;
        
        public SpRangeMove(int mId, CoordY y)
            : base(mId)
        {
            this.y = y;
            
        }
        protected override bool IsYInRange(LightStudio.PokemonBattle.Game.DefContext def)
        {
            return ((def.Defender.OnboardPokemon.CoordY == y || base.IsYInRange(def)));
        }
    }

}
