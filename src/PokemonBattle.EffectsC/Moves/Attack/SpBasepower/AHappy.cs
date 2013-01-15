using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class Happiness : AttackMoveE
  {
    private readonly bool Reverse;

    public Happiness(int moveId, bool reverse)
      : base(moveId)
    {
      Reverse = reverse;
    }

    protected override void CalculateBasePower(DefContext def)
    {
      int v = def.AtkContext.Attacker.Pokemon.Happiness;
      if (Reverse) v = 255 - v;
      def.BasePower = v * 4 / 10;
    }
  }
}
