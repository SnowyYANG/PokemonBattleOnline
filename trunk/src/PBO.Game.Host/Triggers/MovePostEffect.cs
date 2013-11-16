using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class MovePostEffect
  {
    public static void Execute(DefContext def)
    {
      var move = def.AtkContext.Move;

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
        default:
          {
            var aer = def.AtkContext.Attacker;
            if (move.HurtPercentage < 0 && aer.Ability != As.ROCK_HEAD) aer.EffectHurt(-def.Damage * move.HurtPercentage / 100, "m_ReHurt");
            else if (move.MaxHpPercentage < 0) //拼命专用
            {
              var change = aer.Pokemon.MaxHp * move.MaxHpPercentage / 100;
              aer.OnboardPokemon.SetTurnCondition("Assurance");
              aer.ShowLogPm("m_ReHurt");
              aer.Hp += (change == 0 ? -1 : change);
            }
          }
          break;
      }
    }

    private static void DeAbnormalState(DefContext def, PokemonState state)
    {
      if (def.Defender.Hp != 0 && def.Defender.State == state) def.Defender.DeAbnormalState();
    }

    private static void RapidSpin(DefContext def)
    {
      var aer = def.AtkContext.Attacker;
      EHTs.De(aer.Controller.ReportBuilder, aer.Field);
      aer.OnboardPokemon.RemoveCondition("LeechSeed");
      var trap = aer.OnboardPokemon.GetCondition("Trap");
      if (trap != null)
      {
        aer.OnboardPokemon.RemoveCondition("Trap");
        aer.ShowLogPm("TrapFree", trap.Move.Id);
      }
    }

    private static void EatDefenderBerry(DefContext def)
    {
      var i = def.GetCondition<int>("EatenBerry");
      if (i != 0)
      {
        var aer = def.AtkContext.Attacker;
        def.AtkContext.Attacker.ShowLogPm("EatDefenderBerry", i);
        ITs.RaiseItemByMove(aer, i, aer);
      }
    }

    private static void SmackDown(DefContext def)
    {
      var der = def.Defender;
      if (der.Hp != 0 && (der.OnboardPokemon.HasType(BattleType.Flying) || der.Ability == As.LEVITATE))
      {
        der.OnboardPokemon.SetCondition("SmackDown");
        der.ShowLogPm("EnSmackDown");
      }
      if (der.OnboardPokemon.RemoveCondition("MagnetRise")) der.ShowLogPm("DeMagnetRise");
      if (der.OnboardPokemon.RemoveCondition("Telekinesis")) der.ShowLogPm("DeTelekinesis");
    }
  }
}
