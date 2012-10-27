using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
{
  class FSDD : AttackMoveE
  {
    public FSDD(int id)
      : base(id)
    {
    }

    protected override void FilterDefContext(AtkContext atk)
    {
      if (atk.Flag.HasFlag(AtkContextFlag.FSDD)) base.FilterDefContext(atk);
    }
    protected override void Act(AtkContext atk)
    {
      if (atk.Flag.HasFlag(AtkContextFlag.FSDD)) base.Act(atk);
      else
      {
        var tile = MoveE.GetRangeTiles(atk, Move.Range, atk.Attacker.SelectedTarget);
        if (tile.First().HasCondition("FSDD")) FailAll(atk);
        else
        {
          atk.Attacker.AddReportPm("EnFSDD" + Move.Id);
          var c = new Condition();
          c.Turn = atk.Controller.TurnNumber + 2;
          c.Atk = new AtkContext(atk.Attacker, Move) { Attachment = tile };
          tile.First().AddCondition("FSDD", c);
        }
      }
    }
  }
}
