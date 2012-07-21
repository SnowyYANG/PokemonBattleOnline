using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
{
  class HeavySlam:AttackMoveE
  {
    public HeavySlam(int MoveId)
      : base(MoveId)
    { }

    protected override void CalculateBasePower(DefContext def)
    {
      int w = (int)(def.AtkContext.Attacker.Weight / def.Defender.Weight);
      if (w >= 5)
        def.BasePower = 120;
      else if (w >= 4)
        def.BasePower = 100;
      else if (w >= 3)
        def.BasePower = 80;
      else if (w >= 2)
        def.BasePower = 60;
      else
        def.BasePower = 40;
    }
  }
}
