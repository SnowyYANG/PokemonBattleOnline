using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class Acrobatics:AttackMoveE 
  {
    public Acrobatics(int MoveId)
      : base(MoveId)
    { }

    protected override void CalculateBasePower(DefContext def)
    {
      def.BasePower = def.AtkContext.Attacker.Pokemon.Item == null ? 110 : 55;
    }
  }
}
