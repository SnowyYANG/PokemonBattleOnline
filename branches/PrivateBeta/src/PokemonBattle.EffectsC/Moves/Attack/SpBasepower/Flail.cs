using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Flail:AttackMoveE 
  {
    public Flail(int MoveId)
      : base(MoveId)
    { }

    protected override void CalculateBasePower(DefContext def)
    {
      int pwd =def.AtkContext.Attacker.Hp * 48 / def.AtkContext.Attacker.Pokemon.Hp.Origin;

      if (pwd == 1) def.BasePower = 200;
      else if (pwd <= 4) def.BasePower = 150;
      else if (pwd <= 9) def.BasePower = 100;
      else if (pwd <= 16) def.BasePower = 80;
      else if (pwd <= 32) def.BasePower = 40;
      else def.BasePower = 20;
    }
  }
}
