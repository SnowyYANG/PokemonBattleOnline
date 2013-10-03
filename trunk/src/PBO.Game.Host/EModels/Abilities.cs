﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.GameEvents;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
  internal static class ATs
  {
    public static void RaiseAbility(this PokemonProxy pm)
    {
      pm.AddReportPm("Ability", pm.OnboardPokemon.Ability);
    }
    public static bool RaiseAbility(this PokemonProxy pm, int abilityId)
    {
      if (pm.Ability != abilityId) return false;
      RaiseAbility(pm);
      return true;
    }
    public static bool HasProbabilitiedAdditonalEffects(this MoveType move)
    {
      return
        (
        move.Class == MoveInnerClass.AttackWithState ||
        move.Class == MoveInnerClass.AttackWithTargetLv7DChange ||
        move.FlinchProbability > 0 ||
        (move.Attachment != null && move.Attachment.Probability > 0) ||
        (move.Class == MoveInnerClass.AttackWithSelfLv7DChange && move.Lv7DChanges.First().Change > 0)
        );
    }

    public static bool IgnoreDefenderAbility(int ability)
    {
      return ability == As.MOLD_BREAKER || ability == As.TURBOBLAZE || ability == As.TERAVOLT;
    }
    public static bool CannotBeCted(int ability)
    {
      return ability == As.BATTLE_ARMOR || ability == As.SHELL_ARMOR;
    }

    public static bool IgnoreWeather(Controller c)
    {
      foreach (PokemonProxy p in c.ActingPokemons)
      {
        var a = p.Ability;
        if (a == As.AIR_LOCK || a == As.CLOUD_NINE) return true;
      }
      return false;
    }
    public static void Pressure(AtkContext atk, MoveRange range)
    {
      var ts =
        atk.Move.Range == MoveRange.Field || atk.Move.Range == MoveRange.EnemyField ?
        atk.Attacker.Controller.Board[1 - atk.Attacker.Pokemon.TeamId].Pokemons :
        atk.Targets == null ?
        Enumerable.Empty<PokemonProxy>() :
        atk.Targets.Select((t) => t.Defender).Where((p) => p.Pokemon.TeamId != atk.Attacker.Pokemon.TeamId);
      foreach (var d in ts)
        if (d.Ability == As.PRESSURE) atk.Pressure++;     
    }
    public static void Withdrawn(PokemonProxy pm, int ability)
    {
      if (ability == As.REGENERATOR) pm.Pokemon.SetHp(pm.Hp + pm.Pokemon.Hp.Origin / 3);
      else if (ability == As.NATURAL_CURE) pm.Pokemon.State = PokemonState.Normal;
      else if (ability == As.UNNERVE)
        foreach (var p in pm.Controller.GetOnboardPokemons(1 - pm.Pokemon.TeamId)) STs.ItemAttach(p);
    }
    public static void Synchronize(PokemonProxy pm, PokemonProxy by, AttachedState state, int turn)
    {
      if (pm != by && pm.RaiseAbility(As.SYNCHRONIZE)) by.AddState(pm, state, true, turn);
    }
    public static void Defiant(PokemonProxy pm)
    {
      if (pm.CanChangeLv7D(pm, StatType.Atk, 2, false) != 0 && pm.RaiseAbility(As.DEFIANT)) pm.ChangeLv7D(pm, StatType.Atk, 2, false);
    }
    public static void Illusion(PokemonProxy pm)
    {
      if (pm.Ability == As.ILLUSION)
      {
        Pokemon o = pm.Pokemon;
        foreach (Pokemon p in pm.Pokemon.Owner.Pokemons)
          if (p.Hp.Value > 0) o = p;
        if (o != pm.Pokemon) pm.OnboardPokemon.SetCondition("Illusion", o);
      }
    }
    public static void ColorChange(DefContext def)
    {
      var type = def.AtkContext.Type;
      if (type == BattleType.Invalid) type = BattleType.Normal;
      if (
        !def.HitSubstitute &&
        !(def.Defender.OnboardPokemon.Type1 == type && def.Defender.OnboardPokemon.Type2 == BattleType.Invalid) &&
        def.Defender.RaiseAbility(As.COLOR_CHANGE)
        )
      {
        def.Defender.OnboardPokemon.Type1 = type;
        def.Defender.OnboardPokemon.Type2 = BattleType.Invalid;
        def.Defender.AddReportPm("TypeChange", type);
      }
    }
    public static double WeightModifier(PokemonProxy pm)
    {
      double m;
      int id = pm.Ability;
      if (id == As.HEAVY_METAL) m = 2d;
      else if (id == As.LIGHT_METAL) m = 0.5d;
      else m = 1d;
      return m;
    }
    public static bool Stench(DefContext def)
    {
      return def.AtkContext.Attacker.Ability == As.STENCH && def.Defender.Controller.RandomHappen(10);
    }
    public static bool Trace(int abilityId)
    {
      return !(abilityId == As.FORECAST || abilityId == As.ILLUSION || abilityId == As.ZEN_MODE || abilityId == As.MULTITYPE || abilityId == As.TRACE);
    }
    public static void Trace(PokemonProxy sendout)
    {
      int ab = sendout.OnboardPokemon.Ability;
      if (Trace(ab))
        foreach (var pm in sendout.Controller.Board[1 - sendout.Pokemon.TeamId].GetPokemons(sendout.OnboardPokemon.X - 1, sendout.OnboardPokemon.X + 1))
          if (pm.RaiseAbility(As.TRACE))
          {
            pm.ChangeAbility(sendout.OnboardPokemon.Ability);
            pm.Controller.ReportBuilder.ShowLog("Trace", sendout.Id, sendout.OnboardPokemon.Ability);
          }
    }
    public static bool Gluttony(PokemonProxy pm)
    {
      return pm.Hp << 2 <= pm.Pokemon.Hp.Origin || (pm.Ability == As.GLUTTONY && pm.Hp << 1 <= pm.Pokemon.Hp.Origin);
    }

    internal static void SlowStart(Controller Controller)
    {
      foreach (var pm in Controller.ActingPokemons)
        if (pm.Ability == As.SLOW_START)
        {
          int turn = pm.OnboardPokemon.GetCondition<int>("SlowStart");
          if (turn == Controller.TurnNumber)
          {
            pm.OnboardPokemon.RemoveCondition("SlowStart");
            pm.AddReportPm("DeSlowStart");
          }
        }
    }
    internal static void ReTarget(AtkContext atk)
    {
      if (atk.Move.Range == MoveRange.Single)
      {
        int ab = 0;
        if (atk.Type == BattleType.Electric) ab = As.LIGHTNINGROD;
        else if (atk.Type == BattleType.Water) ab = As.STORM_DRAIN;
        if (ab != 0)
          foreach (var pm in atk.Controller.Board.Pokemons)
            if (pm.Ability == ab)
            {
              if (pm != atk.Attacker && pm != atk.Target.Defender)
              {
                pm.RaiseAbility();
                pm.AddReportPm("ReTarget");
                atk.SetTargets(new DefContext[] { new DefContext(atk, pm) });
              }
              return;
            }
      }
    }
    internal static void RecoverAfterMoldBreaker(PokemonProxy pm)
    {
      int id = pm.Ability;
      if (id == As.LIMBER || id == As.OBLIVIOUS || id == As.IMMUNITY || id == As.INSOMNIA || id == As.OWN_TEMPO || id == As.MAGMA_ARMOR || id == As.WATER_VEIL || id == As.VITAL_SPIRIT)
        AbilityAttach.Execute(pm);
    }
    internal static bool Unnerve(PokemonProxy pm)
    {
      return pm.OnboardPokemon.HasCondition("Unnerve") && pm.Ability == As.UNNERVE;
    }
    internal static void AttachUnnerve(Controller c)
    {
      foreach(var pm in c.OnboardPokemons)
        if (!pm.OnboardPokemon.HasCondition("Unnerve") && pm.Ability == As.UNNERVE)
        {
          pm.OnboardPokemon.SetCondition("Unnerve");
          pm.RaiseAbility();
          pm.Controller.ReportBuilder.ShowLog("Unnerve", 1 - pm.Pokemon.TeamId);
        }
    }
    internal static void AttachWeatherObserver(PokemonProxy pm)
    {
      var a = pm.OnboardPokemon.Ability;
      if (a == As.FORECAST || a == As.FLOWER_GIFT) pm.OnboardPokemon.SetCondition("ObserveWeather");
    }

    public static void WeatherChanged(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("ObserveWeather"))
        {
          var ab = pm.Ability;
          if (ab == As.FORECAST || ab == As.FLOWER_GIFT) AbilityAttach.Execute(pm);
        }
    }
    public static void DeIllusion(PokemonProxy pm)
    {
      if (pm.OnboardPokemon.RemoveCondition("Illusion"))
      {
        pm.Controller.ReportBuilder.DeIllusion(pm);
        pm.AddReportPm("DeIllusion");
      }
    }
  }
}
