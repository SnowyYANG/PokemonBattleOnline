using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Triggers
{
  internal static class CanImplement
  {
    public static bool Execute(DefContext def)
    {
      switch (def.Ability)
      {
        case As.VOLT_ABSORB: //10
          return NoEffectWithAbsorb(def, BattleType.Electric);
        case As.WATER_ABSORB: //11
        case As.DRY_SKIN: //87
          return NoEffectWithAbsorb(def, BattleType.Water);
        case As.OBLIVIOUS: //12
          return Oblivious(def);
        case As.FLASH_FIRE: //18
          return FlashFire(def);
        case As.SUCTION_CUPS: //21
          return SuctionCups(def);
        case As.WONDER_GUARD: //25
          return WonderGuard(def);
        case As.LIGHTNINGROD: //31
          return NoEffectWithLv7DUp(def, BattleType.Electric, StatType.SpAtk);
        case As.MOTOR_DRIVE: //78
          return NoEffectWithLv7DUp(def, BattleType.Electric, StatType.Speed);
        case As.STORM_DRAIN: //114
          return NoEffectWithLv7DUp(def, BattleType.Water, StatType.SpAtk);
        case As.SAP_SIPPER: //157
          return NoEffectWithLv7DUp(def, BattleType.Grass, StatType.Atk);
        case As.SOUNDPROOF: //43
          return Soundproof(def);
        case As.STICKY_HOLD: //60
          return StickyHold(def);
        case As.TELEPATHY: //140
          return Telepathy(def);
        default:
          return true;
      }
    }
    private static bool FlashFire(DefContext def)
    {
      var der = def.Defender;
      if (def.AtkContext.Move.Category != MoveCategory.Status && def.AtkContext.Type == BattleType.Fire)
      {
        der.OnboardPokemon.SetCondition("FlashFire");
        der.RaiseAbility();
        der.AddReportPm("FlashFire");
        return false;
      }
      return true;
    }
    private static bool Oblivious(DefContext def)
    {
      if (def.AtkContext.Move.Id == Ms.CAPTIVATE)
      {
        def.Defender.RaiseAbility();
        def.Defender.AddReportPm("NoEffect");
        return false;
      }
      return true;
    }
    private static bool NoEffectWithAbsorb(DefContext def, BattleType type)
    {
      if ((def.AtkContext.Move.Category != MoveCategory.Status || def.AtkContext.Move.Id == Ms.THUNDER_WAVE) && def.AtkContext.Type == type)
      {
        var der = def.Defender;
        der.RaiseAbility();
        if (der.Hp == der.Pokemon.Hp.Origin) der.AddReportPm("NoEffect");
        else der.HpRecoverByOneNth(4);
        return false;
      }
      return true;
    }
    private static bool SuctionCups(DefContext def)
    {
      if (def.AtkContext.Move.Class == MoveInnerClass.ForceToSwitch)
      {
        def.Defender.RaiseAbility();
        def.Defender.AddReportPm("SuctionCups");
        return false;
      }
      return true;
    }
    private static bool WonderGuard(DefContext def)
    {
     var type = def.AtkContext.Type;
      var der = def.Defender;
      if (def.AtkContext.Move.Category == MoveCategory.Status && def.AtkContext.Move.Id != Ms.THUNDER_WAVE || BattleTypeHelper.EffectRevise(type, der.OnboardPokemon.Type1, der.OnboardPokemon.Type2) > 0) return true;
      der.RaiseAbility();
      der.AddReportPm("NoEffect");
      return false;
    }
    private static bool NoEffectWithLv7DUp(DefContext def, BattleType type, StatType stat)
    {
      if (def.AtkContext.Type == type)
      {
        var der = def.Defender;
        der.RaiseAbility();
        if (!der.ChangeLv7D(der, stat, 1, false, null)) der.AddReportPm("NoEffect", null, null);
        return false;
      }
      return true;
    }
    private static bool Soundproof(DefContext def)
    {
      if (def.AtkContext.Move.Flags.IsSound)
      {
          def.Defender.RaiseAbility();
          def.Defender.AddReportPm("NoEffect");
          return false;
      }
      return true;
    }
    private static bool StickyHold(DefContext def)
    {
      var m = def.AtkContext.Move.Id;
      if (m == 168 || m == 271 || m == 343 || m == 415)
      {
          def.Defender.RaiseAbility();
          def.Defender.AddReportPm("NoEffect");
          return false;
      }
      return true;
    }
    private static bool Telepathy(DefContext def)
    {
      var atk = def.AtkContext;
      var der = def.Defender;
      if ((atk.Move.Category != MoveCategory.Status || atk.Move.Id == Ms.THUNDER_WAVE) && atk.Attacker.Pokemon.TeamId == der.Pokemon.TeamId)
      {
          der.RaiseAbility();
          der.AddReportPm("NoEffect");
          return false;
      }
      return true;
    }
  }
}
