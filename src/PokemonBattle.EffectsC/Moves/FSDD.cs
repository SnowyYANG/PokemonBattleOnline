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
      if (atk.HasCondition("FSDD")) base.FilterDefContext(atk);
    }
    protected override void Act(AtkContext atk)
    {
      if (atk.HasCondition("FSDD")) base.Act(atk);
      else
      {
        var tile = MoveE.GetRangeTiles(atk, Move.Range, atk.Attacker.SelectedTarget);
        if (tile.First().HasCondition("FSDD")) atk.FailAll();
        else
        {
          atk.Attacker.AddReportPm("EnFSDD" + Move.Id);
          var c = new Condition();
          c.Turn = atk.Controller.TurnNumber + 2;
          c.Atk = new AtkContext(atk.Attacker) { IgnoreSwitchItem = true };
          c.Atk.SetCondition("FSDD", Move);
          tile.First().AddCondition("FSDD", c);
        }
      }
    }
  }
}
