using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
{
  class FSDD : AttackMoveE
  {
    public FSDD(int id)
      : base(id)
    {
    }
    
    protected override bool HasEffect(DefContext def)
    {
      return true; //this is what a 0D move should be
    }
  }
}
