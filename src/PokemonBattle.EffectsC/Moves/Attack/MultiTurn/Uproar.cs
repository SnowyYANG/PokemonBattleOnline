using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Uproar : AttackMoveE
  {
    public Uproar(int id)
      : base(id)
    {
    }

    public override AtkContext BuildAtkContext(PokemonProxy pm)
    {
      var atk = base.BuildAtkContext(pm);
      atk.Attachment = 3;
      return atk;
    }
  }
}
