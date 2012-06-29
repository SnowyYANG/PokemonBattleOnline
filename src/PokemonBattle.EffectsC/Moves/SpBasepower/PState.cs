using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Effects.Moves
{
  class Hex : AttackMoveE
  {
    public Hex(int MoveId)
      : base(MoveId)
    { }

    protected override void CalculateBasePower(DefContext def)
    {
      if (def.Defender.Pokemon.State != PokemonState.Normal)
        def.BasePower = 100;
      else
        def.BasePower = 50;
    }
  }
}
