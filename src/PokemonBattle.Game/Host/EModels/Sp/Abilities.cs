using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Game.Sp
{
  public static class Abilities
  {
    #region ids
    public const int LIQUID_OOZE = 70;
    public const int STURDY = 138;
    #endregion

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
    public static bool Prankster(this IAbilityE ability)
    {
      return ability.Id == 78;
    }
    public static bool NoGuard(this IAbilityE ability)
    {
      return ability.Id == 85;
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
    public static bool Sniper(this IAbilityE ability)
    {
      return ability.Id == 124;
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

    public static bool HaveCloudNine(Controller c)
    {
      foreach (PokemonProxy p in c.OnboardPokemons)
        if (p.Ability.Id == 14) return true;
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
    public static void CheckTintedLens(DefContext def)
    {
      if (def.EffectRevise < 1 && def.AtkContext.Attacker.Ability.Id == 150)
        def.Damage <<= 1;
    }
    public static void CheckFilterSolidRock(DefContext def)
    {
      if (def.EffectRevise > 1)
      {
        int id = def.Ability.Id;
        if (id == 32 || id == 128)
        {
          def.Damage *= 3;
          def.Damage >>= 2;
        }
      }
    }
    public static void CalculatePowerRevise(DefContext def)
    {
      //如果防御方是耐热特性，攻击方火属性技能威力×0.5。
      const int HEATPROOF = 46;
      //如果防御方是干燥肌肤特性，攻击方火属性技能威力×1.25。 
      const int DRY_SKIN = 25;
      //如果防御方是厚脂肪特性，攻击方火或冰属性技能威力×0.5。 
      const int THICK_FAT = 149;
      BattleType type = def.AtkContext.Type;
      int id = def.Ability.Id;
      if (type == BattleType.Fire)
      {
        if (id == HEATPROOF || id == THICK_FAT) def.PowerRevise = 0.5d;
        else if (id == DRY_SKIN) def.PowerRevise = 1.25d;
      }
      else if (id == THICK_FAT && type == BattleType.Ice) def.PowerRevise = 0.5d;
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
    public static bool CheckSkillLink(AtkContext atk)
    {
      if (atk.Move.MinTimes != atk.Move.MaxTimes && atk.Attacker.Ability.Id == 121)
      {
        atk.Times = 5;
        return true;
      }
      return false;
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
        def.Defender.AddReportPm("TypeChange", def.AtkContext.Type.GetLocalizedName());
      }
    }
  }
}
