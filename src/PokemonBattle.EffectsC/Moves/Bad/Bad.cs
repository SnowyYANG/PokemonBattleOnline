using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
{
  class Bad : StatusMoveE
  {
    public Bad(int id)
      : base(id)
    {
    }
    public override void Execute(AtkContext atk)
    {
      FailAll(atk, "bad", atk.Attacker.Id, Move.Id);
    }
  }
}
