using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
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
          atk.StartExecute(last.Move, atk.Target.Defender.Tile);
          return;
        }
      }
      atk.FailAll();
    }
  }
}
