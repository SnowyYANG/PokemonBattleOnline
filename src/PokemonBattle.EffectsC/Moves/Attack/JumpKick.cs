using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class JumpKick : AttackMoveE
  {
    public JumpKick(int id)
      : base(id)
    {
    }
    
    protected override void CalculateTargets(AtkContext atk)
    {
      base.CalculateTargets(atk);
      if (atk.Targets != null && atk.Target == null) atk.Attacker.EffectHurtByOneNth(2, "FailSelfHurt");
    }
  }
}
