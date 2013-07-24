using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Triggers
{
  internal static class MovePostEffect
  {
    public static void Execute(DefContext def)
    {
      var der = def.Defender;
      var atk = def.AtkContext;
      var move = atk.Move;

      switch (move.Id)
      {
        case Ms.RAPID_SPIN: //229
          RapidSpin(def);
          break;
        case Ms.SMELLINGSALT: //265
          DeAbnormalState(def, PokemonState.PAR);
          break;
        case Ms.WAKEUP_SLAP: //358
          DeAbnormalState(def, PokemonState.SLP);
          break;
        case Ms.PLUCK: //365
        case Ms.BUG_BITE: //450
          EatDefenderBerry(def);
          break;
        case Ms.SMACK_DOWN: //479
          SmackDown(def);
          break;
        case Ms.CIRCLE_THROW: //509
        case Ms.DRAGON_TAIL: //525
          MoveE.ForceSwitch(def);
          break;
      }
    }

    private static void DeAbnormalState(DefContext def, PokemonState state)
    {
      if (def.Defender.State == state) def.Defender.DeAbnormalState();
    }

    private static void RapidSpin(DefContext def)
    {
      var aer = def.AtkContext.Attacker;
      aer.Tile.Field.DeEntryHazards(aer.Controller.ReportBuilder);
      if (aer.OnboardPokemon.HasCondition("LeechSeed")) aer.OnboardPokemon.RemoveCondition("LeechSeed");
      {
        var trap = aer.OnboardPokemon.GetCondition("Trap");
        if (trap != null)
        {
          aer.OnboardPokemon.RemoveCondition("Trap");
          aer.AddReportPm("TrapFree", trap.Move.Id);
        }
      }
    }

    private static void EatDefenderBerry(DefContext def)
    {
      var b = def.GetCondition<int>("EatenBerry");
      if (b != 0)
      {
        var i = Is.BerryNumberToItemId(b);
        var aer = def.AtkContext.Attacker;
        def.AtkContext.Attacker.AddReportPm("EatDefenderBerry", i);
        Is.RaiseItemByMove(aer, i, aer);
      }
    }

    private static void SmackDown(DefContext def)
    {
      var der = def.Defender;
      if (der.OnboardPokemon.HasType(BattleType.Flying) || der.Ability == As.LEVITATE)
      {
        der.OnboardPokemon.SetCondition("SmackDown");
        der.AddReportPm("EnSmackDown");
      }
      if (der.OnboardPokemon.RemoveCondition("MagnetRise")) der.AddReportPm("DeMagnetRise");
      if (der.OnboardPokemon.RemoveCondition("Telekinesis")) der.AddReportPm("DeTelekinesis");
    }
  }
}
