using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  class Encore : StatusMoveE
  {
    public Encore(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      var der = atk.Target.Defender;
      var last = der.LastMove;
      if (last != null && last.Id != 102 && last.Id != 144 && last.Id != 227)
        foreach (var m in der.Moves)
          if (m.Type == last)
          {
            var c = new Condition() { Turn = 3, Move = last };
            if (m.PP != 0 && der.OnboardPokemon.AddCondition("Encore", c))
            {
              der.AddReportPm("EnEncore");
              return;
            }
            break;
          }
      atk.FailAll();
    }
  }
}
