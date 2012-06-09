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
    private readonly CoordY Y;
    private readonly bool Doubled;

    public SpRangeMove(int mId, CoordY y, bool doubled = false)
      : base(mId)
    {
      Y = y;
      Doubled = doubled;
    }
    protected override bool IsYInRange(LightStudio.PokemonBattle.Game.DefContext def)
    {
      return ((def.Defender.OnboardPokemon.CoordY == Y || base.IsYInRange(def)));
    }
    protected override void Calculate(DefContext def)
    {
      base.Calculate(def);
      if (def.Defender.OnboardPokemon.CoordY == Y) def.Damage <<= 1;
    }
  }
}
