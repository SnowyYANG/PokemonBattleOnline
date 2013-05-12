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

    public override void InitAtkContext(AtkContext atk)
    {
      atk.SetCondition("MultiTurn", new Condition() { Turn = 3 });
    }
    protected override void Act(AtkContext atk)
    {
      if (atk.GetCondition("MultiTurn").Turn == 3)
      {
        atk.Attacker.AddReportPm("EnUproar");
        foreach (var p in atk.Controller.Board.Pokemons)
          if (p.State == PokemonState.SLP) p.DeAbnormalState();
      }
      base.Act(atk);
    }
  }
}
