using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class TrumpCard:AttackMoveE 
  {
    public TrumpCard(int MoveId)
      : base(MoveId) { }

    protected override void CalculateBasePower(DefContext def)
    {
      if (def.AtkContext.MoveProxy == null || def.AtkContext.MoveProxy.Type != Move) def.BasePower = 40;
      else
      {
        int pwa = def.AtkContext.MoveProxy.PP;
        if (pwa >= 5) def.BasePower = 40;
        else if (pwa == 4) def.BasePower = 50;
        else if (pwa == 3) def.BasePower = 60;
        else if (pwa == 2) def.BasePower = 80;
        else def.BasePower = 200;
      }
    }
  }
}
