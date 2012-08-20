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
    public const int ARENA_TRAP = 7;
    public const int DRY_SKIN = 25;
    public const int OVERCOAT = 27;
    public const int HEALER = 43;
    public const int HYDRATION = 52;
    public const int ICE_BODY = 54;
    public const int LEVITATE = 66;
    public const int LIQUID_OOZE = 70;
    public const int MAGNET_PULL = 74;
    public const int NATURAL_CURE = 84;
    public const int RAIN_DISH = 102;
    public const int REGENERATOR = 104;
    public const int SAND_FORCE = 110;
    public const int SAND_RUSH = 111;
    public const int SAND_VEIL = 112;
    public const int SHADOW_TAG = 116;
    public const int SHED_SKIN = 117;
    public const int SNOW_CLOAK = 125;
    public const int SOLAR_POWER = 127;
    public const int STURDY = 138;
    #endregion

    #region extension
    public static IEnumerable<PokemonProxy> TeamOnboardPms(this PokemonProxy pm)
    {
      return pm.Controller.GetOnboardPokemons(pm.Pokemon.TeamId);
    }
    public static void RaiseAbility(this PokemonProxy pm)
    {
      if (pm.Ability.Id != 0) pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
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
        move.Class == MoveInnerClass.AttackWithTargetLv7DChange ||
        move.FlinchProbability > 0 ||
        (move.Attachment != null && move.Attachment.Probability > 0) ||
        (move.Class == MoveInnerClass.AttackWithSelfLv7DChange && move.Lv7DChanges.First().Change > 0)
        );
    }
    #endregion

    #region IsXXX
    public static bool Adaptability(this IAbilityE ability)
    {
      return ability.Id == 1;
    }
    public static bool EarlyBird(this IAbilityE ability)
    {
      return ability.Id == 27;
    }
    public static bool SheerForce(this IAbilityE ability)
    {
      return ability.Id == 30;
    }
    public static bool Gluttony(this IAbilityE ability)
    {
      return ability.Id == 40;
    }
    public static bool Guts(this IAbilityE ability)
    {
      return ability.Id == 41;
    }
    public static bool InnerFocus(this IAbilityE ability)
    {
      return ability.Id == 59;
    }
    public static bool Klutz(this IAbilityE ability)
    {
      return ability.Id == 64;
    }
    public static bool MagicGuard(this IAbilityE ability)
    {
      return ability.Id == 71;
    }
    public static bool Prankster(this IAbilityE ability)
    {
      return ability.Id == 78;
    }
    public static bool NoGuard(this IAbilityE ability)
    {
      return ability.Id == 85;
    }
    public static bool QuickFeet(this IAbilityE ability)
    {
      return ability.Id == 101;
    }
    public static bool RockHead(this IAbilityE ability)
    {
      return ability.Id == 106;
    }
    public static bool SereneGrace(this IAbilityE ability)
    {
      return ability.Id == 115;
    }
    public static bool ShieldDust(this IAbilityE ability)
    {
      return ability.Id == 119;
    }
    public static bool Infiltrator(this IAbilityE ability)
    {
      return ability.Id == 122;
    }
    public static bool Stall(this IAbilityE ability)
    {
      return ability.Id == 131;
    }
    public static bool StickyHold(this IAbilityE ability)
    {
      return ability.Id == 136;
    }
    public static bool SuperLuck(this IAbilityE ability)
    {
      return ability.Id == 140;
    }
    public static bool Unaware(this IAbilityE ability)
    {
      return ability.Id == 155;
    }
    public static bool IgnoreDefenderAbility(this IAbilityE ability)
    {
      const int MOLD_BREAKER = 79, TURBOBLAZE = 154, TERAVOLT = 148;
      return ability.Id == MOLD_BREAKER || ability.Id == TURBOBLAZE || ability.Id == TERAVOLT;
    }
    public static bool CannotBeCted(this IAbilityE ability)
    {
      const int BATTLE_ARMOUR = 9, SHELL_ARMOUR = 118;
      return ability.Id == BATTLE_ARMOUR || ability.Id == SHELL_ARMOUR;
    }
    #endregion

    public static bool IgnoreWeather(Controller c)
    {
      const int AIR_LOCK = 3;
      const int CLOUD_NINE = 14;
      foreach (PokemonProxy p in c.OnboardPokemons)
        if (p.Ability.Id == AIR_LOCK || p.Ability.Id == CLOUD_NINE) return true;
      return false;
    }
    public static void Pressure(DefContext def)
    {
      if (def.Defender.Pokemon.TeamId != def.AtkContext.Attacker.Pokemon.TeamId && def.Defender.Ability.Id == 99 && def.AtkContext.MoveProxy.PP > 0) --def.AtkContext.MoveProxy.PP;
    }
    /// <summary>
    /// 已判断过Flinch
    /// </summary>
    public static void Steadfast(PokemonProxy pm)
    {
      if (pm.RaiseAbility(133))
        pm.ChangeLv7D(pm, false, 0, 0, 0, 0, 1);
    }
    public static Modifier TintedLens(DefContext def)
    {
      return (Modifier)(def.EffectRevise < 0 && def.AtkContext.Attacker.Ability.Id == 150 ? 0x2000 : 0x1000);
    }
    public static Modifier FriendGuard(DefContext def)
    {
      Modifier m = 0x1000;
      foreach (PokemonProxy pm in def.Defender.TeamOnboardPms())
        if (pm != def.Defender && pm.Ability.Id == 38) m *= 0xC00;
      return m;
    }
    public static Modifier Sniper(DefContext def)
    {
      return (Modifier)(def.IsCt && def.AtkContext.Attacker.Ability.Id == 124 ? 0x1800 : 0x1000);
    }
    public static Modifier FilterSolidRock(DefContext def)
    {
      if (def.EffectRevise > 0)
      {
        int id = def.Ability.Id;
        if (id == 32 || id == 128) return 0xC00;
      }
      return 0x1000;
    }
    public static void Withdrawn(PokemonProxy pm)
    {
      if (pm.Ability.Id == REGENERATOR) pm.Hp += pm.Pokemon.Hp.Origin / 3;
      else if (pm.Ability.Id == NATURAL_CURE) pm.Pokemon.State = PokemonState.Normal;
    }
    public static Modifier ThickFat(DefContext def)
    {
      BattleType type = def.AtkContext.Move.Type;
      return (Modifier)((type == BattleType.Ice || type == BattleType.Fire) && def.Ability.Id == 149 ? 0x800 : 0x1000);
    }
    public static Modifier PowerModifier(DefContext def)
    {
      //如果防御方是耐热特性，攻击方火属性技能威力×0.5。
      //如果防御方是干燥肌肤特性，攻击方火属性技能威力×1.25。 
      const int HEATPROOF = 46;

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
        foreach (PokemonProxy pm in atk.Attacker.TeamOnboardPms())
          if (pm.Pokemon.PokemonType.Number == 421 && pm.Ability.Id == 35) return m *= 0x1800;
      }
      return m;
    }
    public static Modifier FlowerGift(DefContext def)
    {
      Modifier m = 0x1000;
      if (def.AtkContext.Move.Category == MoveCategory.Special && def.AtkContext.Controller.Weather == Weather.IntenseSunlight)
      {
        foreach (PokemonProxy pm in def.Defender.TeamOnboardPms())
          if (pm.Pokemon.PokemonType.Number == 421 && pm.Ability.Id == 35) m *= 0x1800;
      }
      return m;
    }
    public static bool Normalize(AtkContext atk)
    {
      if (atk.Attacker.Ability.Id == 86)
      {
        atk.Type = BattleType.Normal;
        return true;
      }
      return false;
    }
    /// <summary>
    /// 调用前已判断过 damage > pm.Hp
    /// </summary>
    /// <param name="pm"></param>
    /// <returns></returns>
    public static bool Remain1Hp(PokemonProxy pm)
    {
      return pm.FullHp && pm.RaiseAbility(STURDY);
    }
    public static void Synchronize(PokemonProxy pm, PokemonProxy by, AttachedState state, int turn)
    {
      if (by.CanAddState(pm, state, false) && pm.RaiseAbility(143)) by.AddState(pm, state, false, turn);
    }
    public static void Defiant(PokemonProxy pm)
    {
      if (pm.CanChangeLv7D(pm, StatType.Atk, 2, false) != 0 && pm.RaiseAbility(16)) pm.ChangeLv7D(pm, false, 2);
    }
    public static bool SkillLink(this IAbilityE ability)
    {
      return ability.Id == 121;
    }
    public static void Moxie(DefContext def)
    {
      PokemonProxy pm = def.AtkContext.Attacker;
      if (def.Defender.Hp == 0 && pm.CanChangeLv7D(pm, StatType.Atk, 1, false) != 0 && pm.RaiseAbility(88)) pm.ChangeLv7D(pm, false, 1);
    }
    public static void Illusion(PokemonProxy pm)
    {
      if (pm.Ability.Id == 56)
      {
        Pokemon o = pm.Pokemon;
        foreach (Pokemon p in pm.Pokemon.Owner.Pokemons)
          if (p.Hp.Value > 0) o = p;
        if (o != pm.Pokemon) pm.OnboardPokemon.SetCondition("Illusion", o);
      }
    }
    public static void ColorChange(DefContext def)
    {
      if (!def.HitSubstitute &&
        (def.Defender.OnboardPokemon.Type1 != def.AtkContext.Type || def.Defender.OnboardPokemon.Type2 != BattleType.Invalid) &&
        def.Defender.RaiseAbility(18))
      {
        def.Defender.OnboardPokemon.Type1 = def.AtkContext.Type;
        def.Defender.OnboardPokemon.Type2 = BattleType.Invalid;
        def.Defender.AddReportPm("TypeChange", def.AtkContext.Type);
      }
    }
    public static Modifier Multiscale(DefContext def)
    {
      return (Modifier)(def.Ability.Id == 81 && def.Defender.Hp == def.Defender.Pokemon.Hp.Origin ? 0x800 : 0x1000);
    }
    public static Modifier Hustle(AtkContext atk)
    {
      return (Modifier)(atk.Attacker.Ability.Id == 51 && atk.Move.Category == MoveCategory.Physical ? 0x1800 : 0x1000);
    }
    public static Modifier AccuracyModifier(AtkContext atk)
    {
      const int COMPOUNDEYES = 17, HUSTLE = 51, VICTORY_STAR = 157;
      int ab = atk.Attacker.Ability.Id;
      Modifier m;
      if (ab == COMPOUNDEYES) m = 0x14CC;
      else if (ab == HUSTLE && atk.Move.Category == MoveCategory.Physical) m = 0x1800;
      else m = 0x1000;
      foreach (PokemonProxy pm in atk.Attacker.TeamOnboardPms())
        if (pm.Ability.Id == VICTORY_STAR) m *= 0x1199;
      return m;
    }
    public static double WeightModifier(PokemonProxy pm)
    {
      int HEAVY_METAL = 47, LIGHT_METAL = 67;
      double m;
      int id = pm.Ability.Id;
      if (id == HEAVY_METAL) m = 2d;
      else if (id == LIGHT_METAL) m = 0.5d;
      else m = 1d;
      return m;
    }
    public static bool Stench(DefContext def)
    {
      return def.AtkContext.Attacker.Ability.Id == 135 && def.Defender.Controller.RandomHappen(10);
    }
    public static void PoisonTouch(DefContext def)
    {
      PokemonProxy a = def.AtkContext.Attacker, d = def.Defender;
      if (a.Ability.Id == 95 && def.AtkContext.Move.AdvancedFlags.NeedTouch && d.Controller.RandomHappen(30) && d.CanAddState(a, AttachedState.Poison, false))
      {
        a.RaiseAbility();
        d.AddState(a, AttachedState.Poison, false);
      }
    }
  }
}
