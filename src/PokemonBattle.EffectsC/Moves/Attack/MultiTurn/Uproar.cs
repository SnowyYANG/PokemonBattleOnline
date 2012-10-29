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
      atk.SetCondition("MultiTurn", new Condition() { Turn = 3 });
      return atk;
    }
    protected override void Act(AtkContext atk)
    {
      if (atk.GetCondition("MultiTurn").Turn == 3)
      {
        atk.Attacker.AddReportPm("EnUproar");
        foreach (var t in atk.Controller.Board.Tiles)
          if (t.Pokemon != null && t.Pokemon.State == PokemonState.SLP) t.Pokemon.DeAbnormalState();
      }
      base.Act(atk);
    }
  }
}
