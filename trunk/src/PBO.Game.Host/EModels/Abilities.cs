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
      pm.ShowLogPm("m_Ability", pm.OnboardPokemon.Ability);
    }
    public static bool RaiseAbility(this PokemonProxy pm, int abilityId)
    {
      if (pm.Ability != abilityId) return false;
      RaiseAbility(pm);
      return true;
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
        atk.Move.Range == MoveRange.Board || atk.Move.Range == MoveRange.FoeField ?
        atk.Attacker.Controller.Board[1 - atk.Attacker.Pokemon.TeamId].Pokemons :
        atk.Targets == null ?
        Enumerable.Empty<PokemonProxy>() :
        atk.Targets.Select((t) => t.Defender).Where((p) => p.Pokemon.TeamId != atk.Attacker.Pokemon.TeamId);
      foreach (var d in ts)
        if (d.Ability == As.PRESSURE) atk.Pressure++;     
    }
    public static void Withdrawn(PokemonProxy pm, int ability)
    {
      switch (ability)
      {
        case As.REGENERATOR:
          if (pm.Hp != 0) pm.Pokemon.Hp += pm.Pokemon.MaxHp / 3;
          break;
        case As.NATURAL_CURE:
          if (pm.Hp != 0) pm.Pokemon.State = PokemonState.Normal;
          break;
        case As.UNNERVE:
          foreach (var p in pm.Controller.GetOnboardPokemons(1 - pm.Pokemon.TeamId)) ITs.Attach(p);
          break;
      }
    }
    public static void Synchronize(PokemonProxy pm, PokemonProxy by, AttachedState state, int turn)
    {
      if (pm != by && pm.RaiseAbility(As.SYNCHRONIZE)) by.AddState(pm, state, true, turn);
    }
    public static void Illusion(PokemonProxy pm)
    {
      if (pm.Ability == As.ILLUSION)
      {
        var o = pm.Pokemon.Owner.Pokemons.LastOrDefault((p) => p.Hp > 0);
        if (o != null) pm.OnboardPokemon.SetCondition("Illusion", o.Pokemon);
      }
    }
    public static void ColorChange(DefContext def)
    {
      var type = def.AtkContext.Type;
      if (type == BattleType.Invalid) type = BattleType.Normal;
      if (!def.HitSubstitute) // performance
      {
        var dt = def.Defender.OnboardPokemon.Types;
        if (!(dt.First() == type && dt.Last() == type) && def.Defender.RaiseAbility(As.COLOR_CHANGE))
        {
          def.Defender.OnboardPokemon.SetTypes(type);
          def.Defender.ShowLogPm("TypeChange", (int)type);
        }
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
      return pm.Hp << 2 <= pm.Pokemon.MaxHp || (pm.Ability == As.GLUTTONY && pm.Hp << 1 <= pm.Pokemon.MaxHp);
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
            pm.ShowLogPm("DeSlowStart");
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
                pm.ShowLogPm("ReTarget");
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
        pm.ShowLogPm("DeIllusion");
      }
    }
    public static void StanceChange(PokemonProxy pm)
    {
      if (pm.SelectedMove.Type.Id == Ms.KINGS_SHIELD)
      {
        if (pm.CanChangeForm(681, 0) && RaiseAbility(pm, As.STANCE_CHANGE)) pm.ChangeForm(0, false, "StanceChangeShield");
      }
      else if (pm.SelectedMove.Type.Category != MoveCategory.Status && pm.CanChangeForm(681, 1) && ATs.RaiseAbility(pm, As.STANCE_CHANGE)) pm.ChangeForm(1, false, "StanceChangeSword");
    }
  }
}
