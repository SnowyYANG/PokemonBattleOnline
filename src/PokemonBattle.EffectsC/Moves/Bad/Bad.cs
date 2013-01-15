using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves
{
  class Bad : StatusMoveE
  {
    public Bad(int id)
      : base(id)
    {
    }
    public override void Execute(AtkContext atk)
    {
      atk.FailAll("bad", atk.Attacker.Id, Move.Id);
    }
  }
}
