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
        case Ms.SMELLING_SALTS: //265
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
          if (def.Defender.Hp != 0 && def.Defender.Controller.CanWithdraw(def.Defender)) MoveE.ForceSwitchImplement(def.Defender, def.AtkContext.DefenderAbilityAvailable());
          break;
        default:
          {
            var aer = def.AtkContext.Attacker;
            if (move.HurtPercentage < 0 && !aer.AbilityE(As.ROCK_HEAD)) aer.EffectHurt(-def.Damage * move.HurtPercentage / 100, LogKeys.ReHurt);
            else if (move.MaxHpPercentage < 0) //拼命专用
            {
              var change = aer.Pokemon.MaxHp * move.MaxHpPercentage / 100;
              aer.OnboardPokemon.SetTurnCondition(Cs.Assurance);
              aer.ShowLogPm(LogKeys.ReHurt);
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
      aer.OnboardPokemon.RemoveCondition(Cs.LeechSeed);
      var trap = aer.OnboardPokemon.GetCondition(Cs.Trap);
      if (trap != null)
      {
        aer.OnboardPokemon.RemoveCondition(Cs.Trap);
        aer.ShowLogPm("TrapFree", trap.Move.Id);
      }
    }

    private static void EatDefenderBerry(DefContext def)
    {
      var i = def.GetCondition<int>(Cs.EatenBerry);
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
      if (der.Hp != 0 && (der.OnboardPokemon.HasType(BattleType.Flying) || der.AbilityE(As.LEVITATE)))
      {
        der.OnboardPokemon.SetCondition(Cs.SmackDown);
        der.ShowLogPm("EnSmackDown");
      }
      if (der.OnboardPokemon.RemoveCondition(Cs.MagnetRise)) der.ShowLogPm("DeMagnetRise");
      if (der.OnboardPokemon.RemoveCondition(Cs.Telekinesis)) der.ShowLogPm("DeTelekinesis");
    }
  }
}
