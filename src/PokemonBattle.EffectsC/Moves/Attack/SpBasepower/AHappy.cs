using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Happiness : AttackMoveE
  {
    bool Reverse;

    public Happiness(int moveId, bool reverse)
      : base(moveId)
    {
      Reverse = reverse;
    }

    protected override void CalculateBasePower(DefContext def)
    {
      int v = def.Defender.Pokemon.Happiness;
      if (Reverse) v = 255 - v;
      def.BasePower = v * 4 / 10;
    }
  }
}
