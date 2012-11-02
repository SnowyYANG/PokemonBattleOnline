using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Bide : AttackMoveE
  {
    public Bide(int id)
      : base(id)
    {
    }

    public override void InitAtkContext(AtkContext atk)
    {
      atk.SetCondition("MultiTurn", 3);
      atk.SetCondition("Bide", new Condition());
    }
    protected override void BuildDefContext(AtkContext atk, Tile select)
    {
      if (atk.GetCondition("MultiTurn").Turn == 1)
      {
        var o = atk.GetCondition("Bide");
        var targets = new List<DefContext>();
        if (o.By != null)
        {
          var t = GetRangeTiles(atk, Data.MoveRange.Single, o.By.Tile).FirstOrDefault();
          if (t != null && t.Pokemon != null) targets.Add(new DefContext(atk, t.Pokemon));
        }
        if (!targets.Any()) atk.Attacker.AddReportPm("UseMove", Move); //奇葩的战报
        atk.SetTargets(targets);
      }
    }
    protected override void Act(AtkContext atk)
    {
      var turn = atk.GetCondition("MultiTurn").Turn;
      if (turn == 1)
      {
        atk.Attacker.AddReportPm("DeBide");
        base.Act(atk);
      }
      else if (turn == 2) atk.Attacker.AddReportPm("Bide");
    }
    protected override void CalculateDamages(AtkContext atk)
    {
      atk.Target.Damage = atk.GetCondition("Bide").Damage <<　1;
    }
  }
}
