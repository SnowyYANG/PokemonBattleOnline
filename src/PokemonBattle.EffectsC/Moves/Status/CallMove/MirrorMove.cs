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
    public override void Execute(AtkContext atk)
    {
      if (NotFail(atk))
      {
        var last = atk.Target.Defender.AtkContext;
        if (last != null && last.Move.Flags.Mirrorable)
        {
          CallMove(atk, last.Move, atk.Target.Defender.Tile);
          return;
        }
      }
      FailAll(atk);
    }
  }
}
