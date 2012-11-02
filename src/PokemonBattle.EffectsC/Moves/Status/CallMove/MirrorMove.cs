using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class MirrorMove : StatusMoveE
  {
    public MirrorMove(int id)
      : base(id)
    {
    }
    protected override bool NotFail(AtkContext atk)
    {
      if (base.NotFail(atk))
      {
        var o = atk.Target.Defender.OnboardPokemon.GetCondition("LastMove");
        return o != null && o.Move.Flags.Mirrorable;
      }
      return false;
    }
    public override void Execute(AtkContext atk)
    {
      if (NotFail(atk)) CallMove(atk, atk.Target.Defender.AtkContext.MoveProxy.Type, atk.Target.Defender.Tile);
      else FailAll(atk);
    }
  }
}
