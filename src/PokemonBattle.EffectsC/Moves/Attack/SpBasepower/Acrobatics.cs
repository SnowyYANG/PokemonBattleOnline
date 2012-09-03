using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Acrobatics:AttackMoveE 
  {
    public Acrobatics(int MoveId)
      : base(MoveId)
    { }

    protected override void CalculateBasePower(DefContext def)
    {
      if (def.AtkContext.Attacker.Pokemon.Item == null)
        def.BasePower = 110;
      else
        def.BasePower = 55;
    }
  }
}
