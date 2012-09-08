using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class RapidSpin : AttackMoveE
  {
    public RapidSpin(int id)
      : base(id)
    {
    }
    protected override void ImplementEffect(DefContext def)
    {
      var aer = def.AtkContext.Attacker;
      aer.Controller.Board[aer.Pokemon.TeamId].DeEntryHazards(aer.Controller.ReportBuilder, aer.Pokemon.TeamId);
      if (aer.OnboardPokemon.HasCondition("LeechSeed")) aer.OnboardPokemon.RemoveCondition("LeechSeed");
      {
        var trap = aer.OnboardPokemon.GetCondition("Trap");
        if (trap != null)
        {
          aer.OnboardPokemon.RemoveCondition("Trap");
          aer.AddReportPm("TrapFree", trap.Move.Id);
        }
      }
      base.ImplementEffect(def);
    }
  }
}
