using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
{
  class SkyDrop : AttackMoveE
  {
    public SkyDrop(int id)
      : base(id)
    {
    }
    public override void Execute(AtkContext atk)
    {
      atk.FailAll("bad", atk.Attacker.Id, Move.Id);
    }
  }
}
