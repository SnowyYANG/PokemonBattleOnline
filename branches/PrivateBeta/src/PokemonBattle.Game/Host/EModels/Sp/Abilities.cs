﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;

namespace LightStudio.PokemonBattle.Game.Host.Sp
{
  public static class Abilities
  {
    #region ids
    public const int SPEED_BOOST = 3;
    public const int STURDY = 5;
    public const int DAMP = 6;
    public const int SAND_VEIL = 8;
    public const int FLASH_FIRE = 18;
    public const int SHADOW_TAG = 23;
    public const int WONDER_GUARD = 25;
    public const int LEVITATE = 26;
    public const int NATURAL_CURE = 30;
    public const int TRACE = 36;
    public const int MAGNET_PULL = 42;
    public const int RAIN_DISH = 44;
    public const int PICKUP = 53;
    public const int TRUANT = 54;
    public const int HUSTLE = 55;
    public const int FORECAST = 59;
    public const int STICKY_HOLD = 60;
    public const int SHED_SKIN = 61;
    public const int LIQUID_OOZE = 64;
    public const int ARENA_TRAP = 71;
    public const int STEADFAST = 80;
    public const int SNOW_CLOAK = 81;
    public const int DRY_SKIN = 87;
    public const int POISON_HEAL = 90;
    public const int HYDRATION = 93;
    public const int SOLAR_POWER = 94;
    public const int ICE_BODY = 115;
    public const int MULTITYPE = 121;
    public const int FLOWER_GIFT = 122;
    public const int BAD_DREAMS = 123;
    public const int UNNERVE = 127;
    public const int HEALER = 131;
    public const int HARVEST = 139;
    public const int MOODY = 141;
    public const int OVERCOAT = 142;
    public const int REGENERATOR = 144;
    public const int SAND_RUSH = 146;
    public const int ILLUSION = 149;
    public const int MOXIE = 153;
    public const int MAGIC_BOUNCE = 156;
    public const int SAND_FORCE = 159;
    public const int ZEN_MODE = 161;
    #endregion

    #region extension
    public static void RaiseAbility(this PokemonProxy pm)
    {
      pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
    }
    public static bool RaiseAbility(this PokemonProxy pm, int abilityId)
    {
      if (pm.Ability.Id != abilityId) return false;
      pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
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
    #endregion

    #region IsXXX
    public static bool ShieldDust(this AbilityE ability)
    {
      return ability.Id == 19;
    }
    public static bool SuctionCups(this AbilityE ability)
    {
      return ability.Id == 21;
    }
    public static bool SereneGrace(this AbilityE ability)
    {
      return ability.Id == 32;
    }
    public static bool InnerFocus(this AbilityE ability)
    {
      return ability.Id == 39;
    }
    public static bool SoundProof(this AbilityE ability)
    {
      return ability.Id == 43;
    }
    public static bool EarlyBird(this AbilityE ability)
    {
      return ability.Id == 48;
    }
    public static bool Guts(this AbilityE ability)
    {
      return ability.Id == 62;
    }
    public static bool RockHead(this AbilityE ability)
    {
      return ability.Id == 69;
    }
    public static bool Unburden(this AbilityE ability)
    {
      return ability.Id == 84;
    }
    public static bool Adaptability(this AbilityE ability)
    {
      return ability.Id == 91;
    }
    public static bool SkillLink(this AbilityE ability)
    {
      return ability.Id == 92;
    }
    public static bool QuickFeet(this AbilityE ability)
    {
      return ability.Id == 95;
    }
    public static bool Normalize(this AbilityE ability)
    {
      return ability.Id == 96;
    }
    public static bool MagicGuard(this AbilityE ability)
    {
      return ability.Id == 98;
    }
    public static bool NoGuard(this AbilityE ability)
    {
      return ability.Id == 99;
    }
    public static bool Stall(this AbilityE ability)
    {
      return ability.Id == 100;
    }
    public static bool Klutz(this AbilityE ability)
    {
      return ability.Id == 103;
    }
    public static bool SuperLuck(this AbilityE ability)
    {
      return ability.Id == 105;
    }
    public static bool Unaware(this AbilityE ability)
    {
      return ability.Id == 109;
    }
    public static bool Scrappy(this AbilityE ability)
    {
      return ability.Id == 113;
    }
    public static bool SheerForce(this AbilityE ability)
    {
      return ability.Id == 125;
    }
    public static bool Infiltrator(this AbilityE ability)
    {
      return ability.Id == 151;
    }
    public static bool Prankster(this AbilityE ability)
    {
      return ability.Id == 158;
    }
    public static bool StickyHold(this AbilityE ability)
    {
      return ability.Id == STICKY_HOLD;
    }
    public static bool IgnoreDefenderAbility(this AbilityE ability)
    {
      const int MOLD_BREAKER = 104, TURBOBLAZE = 163, TERAVOLT = 164;
      return ability.Id == MOLD_BREAKER || ability.Id == TURBOBLAZE || ability.Id == TERAVOLT;
    }
    public static bool CannotBeCted(this AbilityE ability)
    {
      const int BATTLE_ARMOUR = 4, SHELL_ARMOUR = 75;
      return ability.Id == BATTLE_ARMOUR || ability.Id == SHELL_ARMOUR;
    }
    #endregion

    #region public sp triggers
    public static bool IgnoreWeather(Controller c)
    {
      const int CLOUD_NINE = 13, AIR_LOCK = 76;
      foreach (PokemonProxy p in c.ActingPokemons)
      {
        var a = p.Ability.Id;
        if (a == AIR_LOCK || a == CLOUD_NINE) return true;
      }
      return false;
    }
    public static void Pressure(AtkContext atk, MoveRange range)
    {
      const int PRESSURE = 46;
      var ts =
        atk.Move.Range == MoveRange.Field || atk.Move.Range == MoveRange.EnemyField ?
        atk.Attacker.Controller.Board[1 - atk.Attacker.Pokemon.TeamId].Pokemons :
        atk.Targets == null ?
        Enumerable.Empty<PokemonProxy>() :
        atk.Targets.Select((t) => t.Defender).Where((p) => p.Pokemon.TeamId != atk.Attacker.Pokemon.TeamId);
      foreach (var d in ts)
        if (d.Ability.Id == PRESSURE) atk.Pressure++;     
    }
    public static void Withdrawn(PokemonProxy pm, int ability)
    {
      if (ability == REGENERATOR) pm.Pokemon.SetHp(pm.Hp + pm.Pokemon.Hp.Origin / 3);
      else if (ability == NATURAL_CURE) pm.Pokemon.State = PokemonState.Normal;
      else if (ability == UNNERVE)
        foreach (var p in pm.Controller.GetOnboardPokemons(1 - pm.Pokemon.TeamId)) p.Item.Attach(p);
    }
    public static Modifier ThickFat(DefContext def)
    {
      const int THICK_FAT = 47;
      BattleType type = def.AtkContext.Move.Type;
      return (Modifier)((type == BattleType.Ice || type == BattleType.Fire) && def.Ability.Id == THICK_FAT ? 0x800 : 0x1000);
    }
    public static Modifier PowerModifier(DefContext def)
    {
      //如果防御方是耐热特性，攻击方火属性技能威力×0.5。
      //如果防御方是干燥肌肤特性，攻击方火属性技能威力×1.25。 
      const int HEATPROOF = 85;

      AtkContext atk = def.AtkContext;

      int id = def.Ability.Id;
      ushort d = 0x1000;
      if (atk.Type == BattleType.Fire)
      {
        if (id == HEATPROOF) d = 0x800;
        else if (id == DRY_SKIN) d = 0x1800;
      }
      Modifier r = d;
      if (atk.Move.HasProbabilitiedAdditonalEffects() && atk.Attacker.Ability.SheerForce()) r *= 0x14cd;
      return r;
    }
    public static Modifier FlowerGift(AtkContext atk)
    {
      Modifier m = 0x1000;
      if (atk.Move.Category == MoveCategory.Physical && atk.Controller.Weather == Weather.IntenseSunlight)
      {
        foreach (PokemonProxy pm in atk.Controller.GetOnboardPokemons(atk.Attacker.Pokemon.Id))
          if (pm.Pokemon.Form.Type.Number == 421 && pm.Ability.Id == FLOWER_GIFT) return m *= 0x1800;
      }
      return m;
    }
    public static Modifier DModifier(DefContext def)
    {
      var der = def.Defender;
      Modifier m = (ushort)(der.Ability.Id == 63 && der.State != PokemonState.Normal? 0x1800 : 0x1000);
      if (def.AtkContext.Move.Category == MoveCategory.Special && der.Controller.Weather == Weather.IntenseSunlight)
      {
        foreach (PokemonProxy pm in der.Controller.GetOnboardPokemons(der.Pokemon.TeamId))
          if (pm.Pokemon.Form.Type.Number == 421 && pm.Ability.Id == FLOWER_GIFT) m *= 0x1800;
      }
      return m;
    }
    public static void Synchronize(PokemonProxy pm, PokemonProxy by, AttachedState state, int turn)
    {
      const int SYNCHRONIZE = 28;
      if (pm.RaiseAbility(SYNCHRONIZE)) by.AddState(pm, state, true, turn);
    }
    public static void Defiant(PokemonProxy pm)
    {
      const int DEFIANT = 128;
      if (pm.CanChangeLv7D(pm, StatType.Atk, 2, false) != 0 && pm.RaiseAbility(DEFIANT)) pm.ChangeLv7D(pm, false, 2);
    }
    public static void Illusion(PokemonProxy pm)
    {
      if (pm.Ability.Id == ILLUSION)
      {
        Pokemon o = pm.Pokemon;
        foreach (Pokemon p in pm.Pokemon.Owner.Pokemons)
          if (p.Hp.Value > 0) o = p;
        if (o != pm.Pokemon) pm.OnboardPokemon.SetCondition("Illusion", o);
      }
    }
    public static void ColorChange(DefContext def)
    {
      const int COLOR_CHANGE = 16;
      var type = def.AtkContext.Type;
      if (type == BattleType.Invalid) type = BattleType.Normal;
      if (
        !def.HitSubstitute &&
        !(def.Defender.OnboardPokemon.Type1 == type && def.Defender.OnboardPokemon.Type2 == BattleType.Invalid) &&
        def.Defender.RaiseAbility(COLOR_CHANGE)
        )
      {
        def.Defender.OnboardPokemon.Type1 = type;
        def.Defender.OnboardPokemon.Type2 = BattleType.Invalid;
        def.Defender.AddReportPm("TypeChange", type);
      }
    }
    public static Modifier Hustle(AtkContext atk)
    {
      return (Modifier)(atk.Attacker.Ability.Id == HUSTLE && atk.Move.Category == MoveCategory.Physical ? 0x1800 : 0x1000);
    }
    public static Modifier AccuracyModifier(AtkContext atk)
    {
      const int COMPOUNDEYES = 14, VICTORY_STAR = 162;
      int ab = atk.Attacker.Ability.Id;
      Modifier m;
      if (ab == COMPOUNDEYES) m = 0x14CC;
      else if (ab == HUSTLE && atk.Move.Category == MoveCategory.Physical) m = 0x1800;
      else m = 0x1000;
      foreach (PokemonProxy pm in atk.Controller.GetOnboardPokemons(atk.Attacker.Pokemon.TeamId))
        if (pm.Ability.Id == VICTORY_STAR) m *= 0x1199;
      return m;
    }
    public static double WeightModifier(PokemonProxy pm)
    {
      int HEAVY_METAL = 134, LIGHT_METAL = 135;
      double m;
      int id = pm.Ability.Id;
      if (id == HEAVY_METAL) m = 2d;
      else if (id == LIGHT_METAL) m = 0.5d;
      else m = 1d;
      return m;
    }
    public static bool Stench(DefContext def)
    {
      const int STENCH = 1;
      return def.AtkContext.Attacker.Ability.Id == STENCH && def.Defender.Controller.RandomHappen(10);
    }
    public static void PoisonTouch(DefContext def)
    {
      const int POISON_TOUCH = 143;
      PokemonProxy a = def.AtkContext.Attacker, d = def.Defender;
      if (a.Ability.Id == POISON_TOUCH && def.AtkContext.Move.Flags.NeedTouch && d.Controller.RandomHappen(30) && d.CanAddState(a, AttachedState.PSN, false))
      {
        a.RaiseAbility();
        d.AddState(a, AttachedState.PSN, false);
      }
    }
    public static bool Trace(int abilityId)
    {
      return !(abilityId == FORECAST || abilityId == ILLUSION || abilityId == ZEN_MODE || abilityId == MULTITYPE || abilityId == TRACE);
    }
    public static void Trace(PokemonProxy sendout)
    {
      int ab = sendout.OnboardPokemon.Ability;
      if (Trace(ab))
        foreach (var pm in sendout.Controller.Board[1 - sendout.Pokemon.TeamId].GetPokemons(sendout.OnboardPokemon.X - 1, sendout.OnboardPokemon.X + 1))
          if (pm.RaiseAbility(TRACE)) pm.ChangeAbility(sendout.OnboardPokemon.Ability, "Trace", sendout.Id);
    }
    public static bool Gluttony(PokemonProxy pm)
    {
      const int GLUTTONY = 82;
      return pm.Hp << 2 < pm.Pokemon.Hp.Origin || (pm.Ability.Id == GLUTTONY && pm.Hp << 1 < pm.Pokemon.Hp.Origin);
    }
    #endregion

    #region internal sp triggers
    internal static void SlowStart(Controller Controller)
    {
      const int SLOW_START = 112;
      foreach (var pm in Controller.ActingPokemons)
        if (pm.Ability.Id == SLOW_START)
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
        const int LIGHTNINGROD = 31, STORM_DRAIN = 114;
        int ab = 0;
        if (atk.Type == BattleType.Electric) ab = LIGHTNINGROD;
        else if (atk.Type == BattleType.Water) ab = STORM_DRAIN;
        if (ab != 0)
          foreach (var pm in atk.Controller.Board.Pokemons)
            if (pm.Ability.Id == ab)
            {
              if (pm != atk.Target.Defender)
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
      const int LIMBER = 7, OBLIVIOUS = 12, IMMUNITY = 17, INSOMNIA = 15, OWN_TEMPO = 20, MAGMA_ARMOUR = 40, WATER_VEIL = 41, VITAL_SPIRIT = 72;
      var ability = pm.Ability;
      int id = ability.Id;
      if (id == LIMBER || id == OBLIVIOUS || id == IMMUNITY || id == INSOMNIA || id == OWN_TEMPO || id == MAGMA_ARMOUR || id == WATER_VEIL || id == VITAL_SPIRIT)
        ability.Attach(pm);
    }
    internal static Modifier DamageFinalModifier(DefContext def)
    {
      const int MULTISCALE = 136, TINTED_LENS = 110, FRIEND_GUARD = 132, SNIPER = 97, FILTER = 111, SOLID_ROCK = 116;
      var der = def.Defender;
      int aa = def.AtkContext.Attacker.Ability.Id, da = def.Ability.Id;
      //If the target's ability is Multiscale and the target is at full health.
      Modifier m = (Modifier)(da == MULTISCALE && der.Hp == der.Pokemon.Hp.Origin ? 0x800 : 0x1000);
      //If the user's ability is Tinted Lens and the move wasn't very effective.
      if (def.EffectRevise < 0 && aa == TINTED_LENS) m *= 0x2000;
      //If one of the target's allies' ability is Friend Guard.
      foreach (PokemonProxy pm in der.Controller.GetOnboardPokemons(der.Pokemon.TeamId))
        if (pm != der && pm.Ability.Id == FRIEND_GUARD) m *= 0xC00;
      //If user has ability Sniper and move was a critical hit.
      if (def.IsCt && aa == SNIPER) m *= 0x1800;
      //If the target's ability is Solid Rock or Filter and the move was super effective.
      if (def.EffectRevise > 0 && (da == FILTER || da == SOLID_ROCK)) m *= 0xC00;
      return m;
    }
    internal static bool Unnerve(PokemonProxy pm)
    {
      return pm.OnboardPokemon.HasCondition("Unnerve") && pm.Ability.Id == UNNERVE;
    }
    internal static void AttachUnnerve(Controller c)
    {
      foreach(var pm in c.OnboardPokemons)
        if (!pm.OnboardPokemon.HasCondition("Unnerve") && pm.Ability.Id == UNNERVE)
        {
          pm.OnboardPokemon.SetCondition("Unnerve");
          pm.RaiseAbility();
          pm.Controller.ReportBuilder.Add("Unnerve", 1 - pm.Pokemon.TeamId);
        }
    }
    internal static void AttachWeatherObserver(PokemonProxy pm)
    {
      var a = pm.OnboardPokemon.Ability;
      if (a == FORECAST || a == FLOWER_GIFT) pm.OnboardPokemon.SetCondition("ObserveWeather");
    }
    #endregion
    public static void WeatherChanged(Controller c)
    {
      foreach (var pm in c.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("ObserveWeather"))
        {
          var ab = pm.Ability;
          if (ab.Id == FORECAST || ab.Id == FLOWER_GIFT) ab.Attach(pm);
        }
    }
  }
}