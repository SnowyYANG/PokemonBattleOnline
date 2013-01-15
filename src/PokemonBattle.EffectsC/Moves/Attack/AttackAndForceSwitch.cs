using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class AttackAndForceSwitch : AttackMoveE
  {
    public AttackAndForceSwitch(int id)
      : base(id)
    {
    }
    
    protected override void PostEffect(DefContext def)
    {
      MoveE.ForceSwitch(def);
    }
  }
}
