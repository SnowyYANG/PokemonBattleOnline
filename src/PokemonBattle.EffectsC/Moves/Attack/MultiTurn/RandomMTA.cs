using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ms = LightStudio.PokemonBattle.Game.Host.Sp.Moves;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class RandomMTA : AttackMoveE
  {
    public RandomMTA(int id)
      : base(id)
    {
    }

    public override AtkContext BuildAtkContext(PokemonProxy pm)
    {
      var atk = base.BuildAtkContext(pm);
      atk.Attachment = atk.Controller.GetRandomInt(2, 3);
      return atk;
    }
  }
}
