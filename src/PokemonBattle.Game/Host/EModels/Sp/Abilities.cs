using System;
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
    public const int LEVITATE = 66;
    public const int LIQUID_OOZE = 70;
    public const int STURDY = 138;
    #endregion

    public static IEnumerable<PokemonProxy> TeamOnboardPms(this PokemonProxy pm)
    {
      return pm.Controller.GetOnboardPokemons(pm.Pokemon.TeamId);
    }
    public static bool RaiseAbility(this PokemonProxy pm, int abilityId)
    {
      if (pm.Ability.Id != abilityId) return false;
      pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
      return true;
    }

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
    public static bool Klutz(this IAbilityE ability)
    {
      return ability.Id == 64;
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
    public static bool Infiltrator(this IAbilityE ability)
    {
      return ability.Id == 122;
    }
    public static bool Stall(this IAbilityE ability)
    {
      return ability.Id == 131;
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
    public static void CheckPressure(DefContext def)
    {
      if (def.Defender.Ability.Id == 99)
        --def.AtkContext.MoveProxy.PP;
    }
    /// <summary>
    /// 已判断过Flinch
    /// </summary>
    public static void CheckSteadfast(PokemonProxy pm)
    {
      if (pm.RaiseAbility(133))
        pm.ChangeLv7D(pm, 0, 0, 0, 0, 1);
    }
    public static Modifier TintedLens(DefContext def)
    {
      if (def.EffectRevise < 0 && def.AtkContext.Attacker.Ability.Id == 150)
        return 0x2000;
      return 0x1000;
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
      if (def.IsCt && def.AtkContext.Attacker.Ability.Id == 124)
        return 0x1800;
      return 0x1000;
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
#warning SimGame更新
      const int NATURAL_CURE = 84;
      const int REGENERATOR = 104;
      if (pm.Ability.Id == NATURAL_CURE) pm.Hp += pm.Pokemon.Hp.Origin / 3;
      else if (pm.Ability.Id == REGENERATOR) pm.Pokemon.State = PokemonState.Normal;
    }
    public static Modifier ThickFat(DefContext def)
    {
      BattleType type = def.AtkContext.Move.Type;
      if ((type == BattleType.Ice || type == BattleType.Fire) && def.Ability.Id == 149)
        return 0x800;
      return 0x1000;
    }
    public static Modifier PowerModifier(DefContext def)
    {
      //如果防御方是耐热特性，攻击方火属性技能威力×0.5。
      const int HEATPROOF = 46;
      //如果防御方是干燥肌肤特性，攻击方火属性技能威力×1.25。 
      const int DRY_SKIN = 25;

      int id = def.Ability.Id;
      ushort d = 0x1000;
      if (def.AtkContext.Type == BattleType.Fire)
      {
        if (id == HEATPROOF) d = 0x800;
        else if (id == DRY_SKIN) d = 0x1800;
      }
      Modifier r = d;
      if (def.AtkContext.SheerForceActive) r *= 0x14cd;
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
    public static bool CalculateType(AtkContext atk)
    {
      const int NORMALIZE = 86;
      if (atk.Attacker.Ability.Id == NORMALIZE)
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
      if (pm.Ability.Id == STURDY && !pm.Pokemon.Hp.IsChanged)
      {
        pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
        return true;
      }
      return false;
    }
    public static void CheckSynchronize(PokemonProxy pm, PokemonProxy by, AttachedState state, int turn)
    {
      if (pm.Ability.Id == 143)
        by.AddState(pm, state, turn);
    }
    public static void CheckDefiant(PokemonProxy pm)
    {
      if (pm.Ability.Id == 16)
      {
        pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
        pm.ChangeLv7D(pm, 2);
      }
    }
    public static bool SkillLink(this IAbilityE ability)
    {
      return ability.Id == 121;
    }
    public static void CheckMoxie(DefContext def)
    {
      PokemonProxy pm = def.AtkContext.Attacker;
      if (def.Defender.Hp == 0 && pm.Ability.Id == 88)
        pm.ChangeLv7D(pm, 1);
    }
    public static void CheckIllusion(PokemonProxy pm)
    {
      if (pm.Ability.Id == 56)
      {
        Pokemon o = pm.Pokemon;
        foreach (Pokemon p in pm.Pokemon.Owner.Pokemons)
          if (p.Hp.Value > 0) o = p;
        if (o != pm.Pokemon)
          pm.OnboardPokemon.SetCondition("Illusion", o);
      }
    }
    public static void CheckColorChange(DefContext def)
    {
      if (!def.HitSubstitute && def.Defender.Ability.Id == 18)
      {
        def.Defender.OnboardPokemon.Type1 = def.AtkContext.Type;
        def.Defender.OnboardPokemon.Type2 = BattleType.Invalid;
        def.Defender.Controller.ReportBuilder.Add(new AbilityEvent(def.Defender));
        def.Defender.AddReportPm("TypeChange", def.AtkContext.Type);
      }
    }

    public static Modifier Multiscale(DefContext def)
    {
      if (def.Ability.Id == 81 && def.Defender.Hp == def.Defender.Pokemon.Hp.Origin)
        return 0x800;
      return 0x1000;
    }
    public static Modifier Hustle(AtkContext atk)
    {
      if (atk.Attacker.Ability.Id == 51 && atk.Move.Category == MoveCategory.Physical)
        return 0x800;
      return 0x1000;
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
  }
}
