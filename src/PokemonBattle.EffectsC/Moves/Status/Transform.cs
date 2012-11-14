using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Transform : StatusMoveE
  {
    public Transform(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      if (atk.Attacker.CanTransform(atk.Target.Defender)) atk.Attacker.Transform(atk.Target.Defender);
      else FailAll(atk);
    }
  }
}
