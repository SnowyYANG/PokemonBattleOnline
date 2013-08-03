﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class MoveImplementEffect
  {
    public static void Execute(DefContext def)
    {
      var der = def.Defender;
      var atk = def.AtkContext;
      var aer = atk.Attacker;
      var move = atk.Move;

      switch (move.Id)
      {
        case Ms.TRI_ATTACK: //161
          TriAttack(def);
          break;
        case Ms.THIEF: //168
        case Ms.COVET: //343
          Thief(def);
          break;
        case Ms.KNOCK_OFF: //282
          RemoveItem(def, true, "KnockOff");
          break;
        case Ms.INCINERATE: //510
          RemoveItem(def, false, "Incinerate");
          break;
        case Ms.SECRET_POWER: //290
          SecretPower(def);
          break;
        case Ms.NATURAL_GIFT: //363
          def.AtkContext.Attacker.ConsumeItem();
          break;
        case Ms.PLUCK: //365
        case Ms.BUG_BITE: //450
          EatDefenderBerry(def);
          break;
        case Ms.FLING: //374
          {
            var i = aer.Pokemon.Item.Id;
            aer.ConsumeItem();
            Is.RaiseItemByMove(def.Defender, i, aer);
          }
          break;
        case Ms.CHATTER: //448
          Chatter(def);
          break;
        case Ms.SHADOW_FORCE: //467
          if (def.Defender.OnboardPokemon.RemoveCondition("Protect")) def.Defender.AddReportPm("DeProtect");
          break;
        case Ms.CLEAR_SMOG: //499
          der.OnboardPokemon.SetLv7D(0, 0, 0, 0, 0, 0, 0);
          der.AddReportPm("7DReset");
          break;
        default:
          if (der.Hp > 0)
          {
            if (move.Class == MoveInnerClass.AttackWithTargetLv7DChange) der.ChangeLv7D(def);
            else if (move.Class == MoveInnerClass.AttackWithState) der.AddState(def);
            if (def.Ability != As.INNER_FOCUS && (move.FlinchProbability != 0 && def.RandomHappen(move.FlinchProbability) || As.Stench(def) || Is.CanAttackFlinch(def))) der.OnboardPokemon.SetTurnCondition("Flinch");
          }
          break;
      }
      switch (move.Id)
      {
        case Ms.RELIC_SONG:
        if (aer.CanChangeForm(648)) aer.ChangeForm(1 - aer.OnboardPokemon.Form.Index);
          break;
      }
    }

    private static void TriAttack(DefContext def)
    {
      if (def.RandomHappen(20))
      {
        int i = def.Defender.Controller.GetRandomInt(0, 2);
        AttachedState s = i == 0 ? AttachedState.PAR : i == 1 ? AttachedState.BRN : AttachedState.FRZ;
        def.Defender.AddState(def.AtkContext.Attacker, s, false);
      }
    }

    private static void Thief(DefContext def)
    {
      var aer = def.AtkContext.Attacker;
      if (def.AtkContext.HasCondition("Thief") && Is.CanLostItem(def.Defender))
      {
        var i = def.Defender.Pokemon.Item.Id;
        def.Defender.RemoveItem();
        aer.SetItem(i, "Thief", def.Defender, false); //先铁棘再果子
      }
    }

    private static void SecretPower(DefContext def)
    {
      if (def.RandomHappen(30))
        switch (def.Defender.Controller.GameSettings.Terrain)
        {
          case Terrain.Path:
            def.Defender.AddState(def.AtkContext.Attacker, Data.AttachedState.PAR, false);
            break;
        }
    }

    private static void Chatter(DefContext def)
    {
      var chatter = def.AtkContext.Attacker.Pokemon.Chatter;
      if (chatter != null && Math.Abs(chatter.GetHashCode()) % 3 != 1 && def.RandomHappen(10))
        def.Defender.AddState(def.AtkContext.Attacker, Data.AttachedState.Confuse, false);
    }

    private static void EatDefenderBerry(DefContext def)
    {
      if (Is.CanLostItem(def.Defender))
      {
        var i = def.Defender.Pokemon.Item.Id;
        if (Is.BerryNumber(i) != 0) def.SetCondition("EatenBerry", i);
      }
    }

    private static void RemoveItem(DefContext def, bool berry, string log)
    {
      var der = def.Defender;
      if (Is.CanLostItem(der))
      {
        var i = der.Pokemon.Item.Id;
        if (!berry || Is.BerryNumber(i) != 0)
        {
          der.RemoveItem();
          if (log != null) der.Controller.ReportBuilder.Add(new GameEvents.RemoveItem(log, der, i, berry ? null : (ValueType)def.AtkContext.Attacker.Id));
        }
      }
    }
  }
}
