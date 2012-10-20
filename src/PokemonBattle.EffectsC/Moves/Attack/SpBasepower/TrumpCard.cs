using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class TrumpCard:AttackMoveE 
  {
    public TrumpCard(int MoveId)
      : base(MoveId) { }

    protected override void CalculateBasePower(DefContext def)
    {
      int pwa = def.AtkContext.MoveProxy.PP;
      if (pwa >= 5 || def.AtkContext.Move != Move) def.BasePower = 40;
      else if (pwa == 4) def.BasePower = 50;
      else if (pwa == 3) def.BasePower = 60;
      else if (pwa == 2) def.BasePower = 80;
      else def.BasePower = 200;
    }
  }
}
