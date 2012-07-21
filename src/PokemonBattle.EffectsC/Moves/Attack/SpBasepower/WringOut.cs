using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
{
  class WringOut:AttackMoveE
  {
    public WringOut(int MoveId)
      : base(MoveId)
    { }

    protected override void CalculateBasePower(DefContext def)
    {
      int pwd = (int)(120 * def.Defender.Hp / def.Defender.Pokemon.Hp.Origin);
      if (pwd == 0)
        def.BasePower = 1;
      else
        def.BasePower = pwd;
    }
  }
}
