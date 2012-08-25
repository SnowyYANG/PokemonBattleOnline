﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Sp
{
  public static class Items
  {
    #region ids
    public const int LEFTOVERS = 18;
    public const int TOXIC_ORB = 49;
    public const int FLAME_ORB = 50;
    public const int BLACK_SLUDGE = 58;
    public const int STICKY_BARB = 65;
    public const int RED_CARD = 106;
    public const int EJECT_BUTTON = 111;
    private const int LIFE_ORB = 47;
    #endregion

    public static void RaiseItem(this PokemonProxy pm, string key = null)
    {
      pm.UsingItem = false;
      EffectsService.GetItem(pm.Pokemon.Item).Raise(pm, key);
    }

    public static bool GripClaw(this IItemE item)
    {
      return item.Id == 63;
    }
    public static bool ShedShell(this IItemE item)
    {
      return item.Id == 72;
    }
    public static bool BigRoot(this IItemE item)
    {
      return item.Id == 73;
    }
    public static bool BindingBand(this IItemE item)
    {
      return item.Id == 108;
    }
    public static bool Mail(this Item item)
    {
      return item.Id == 194;
    }

    public static void WhiteHerb(PokemonProxy pm)
    {
      if (pm.Item.Id == 5)
      {
        SixD lvs = pm.OnboardPokemon.Lv5D as SixD;
        bool raise = false;
        if (lvs.Atk < 0) { lvs.Atk = 0; raise = true; }
        if (lvs.Def < 0) { lvs.Def = 0; raise = true; }
        if (lvs.SpAtk < 0) { lvs.SpAtk = 0; raise = true; }
        if (lvs.SpDef < 0) { lvs.SpDef = 0; raise = true; }
        if (lvs.Speed < 0) { lvs.Speed = 0; raise = true; }
        if (pm.OnboardPokemon.AccuracyLv < 0) { pm.OnboardPokemon.AccuracyLv = 0; raise = true; }
        if (pm.OnboardPokemon.EvasionLv < 0) { pm.OnboardPokemon.EvasionLv = 0; raise = true; }
        if (raise) RaiseItem(pm, "WhiteHerb");
      }
    }
    public static Modifier WideLens(AtkContext atk)
    {
      return (Modifier)(atk.Attacker.Item.Id == 42 ? 0x1199 : 0x1000);
    }
    public static Modifier ZoomLens(DefContext target)
    {
      return (Modifier)(target.AtkContext.Attacker.Item.Id == 53 && target.AtkContext.Attacker.LastActTurn == target.Defender.LastActTurn ? 0x1333 : 0x1000);
    }
    public static Modifier AccuracyModifier(DefContext def)
    {
      const int BRIGHT_POWDER = 4, LAX_INCENSE = 37;
      int i = def.Defender.Item.Id;
      ushort r = 0x1000;
      if (i == BRIGHT_POWDER) r = 0xE66;
      else if (i == LAX_INCENSE) r = 0xF34;
      return r;
    }
    public static Modifier DamageFinalModifier(DefContext target)
    {
      //If the user is holding the item Metronome. If n is the number of time the current move was used successfully and successively, the value of the modifier is 0x1000+n*0x333 if n≤4 and 0x2000 otherwise.
      const int METRONOME = 54;
      //If the user is holding an expert belt and the move was super effective.
      const int EXPERT_BELT = 45;
      //If the user is holding a Life Orb.

      PokemonProxy aer = target.AtkContext.Attacker;
      int item = aer.Item.Id;
      ushort r = 0x1000;
      switch (item)
      {
        case EXPERT_BELT:
          if (target.EffectRevise > 0) r = 0x1333;
          break;
        case LIFE_ORB:
          r = 0x14cc;
          break;
      }
      return r;
    }
    /// <summary>
    /// 气球的提示信息不是Attach而是Debut，是唯一会Debut的道具
    /// </summary>
    public static void AirBalloon(PokemonProxy pm)
    {
      if (pm.Item.Id == 105) pm.AddReportPm("EnBalloon");
    }
    public static bool AirBalloon(this IItemE item)
    {
      return item.Id == 105;
    }

    /// <summary>
    /// 调用前已判断过 damage > pm.Hp
    /// </summary>
    /// <param name="pm"></param>
    /// <returns></returns>
    public static bool Remain1Hp(PokemonProxy pm)
    {
      const int FOCUS_BAND = 15, FOCUS_SASH = 52;
      if ((pm.Item.Id == FOCUS_BAND && pm.Controller.OneNth(10)) || (pm.Item.Id == FOCUS_SASH && pm.Hp == pm.Pokemon.Hp.Origin))
      {
        pm.RaiseItem("FocusItem");
        return true;
      }
      return false;
    }
    public static void AttackPostEffect(PokemonProxy pm)
    {
      const int SHELL_BELL = 35;
      if (pm.Item.Id == SHELL_BELL) pm.HpRecoverByOneNth(8, "ItemRecover", SHELL_BELL);
      else if (pm.Item.Id == LIFE_ORB)
      {
        pm.EffectHurtByOneNth(10, "LifeOrb");
        pm.CheckFaint();
      }
    }
    public static bool CanAttackFlinch(DefContext def)
    {
      const int KINGS_ROCK = 10, RAZOR_FANG = 97;
      int item = def.AtkContext.Attacker.Item.Id;
      return (item == KINGS_ROCK || item == RAZOR_FANG) && def.Defender.Controller.RandomHappen(10);
    }
    public static bool PowerHerb(PokemonProxy pm)
    {
      if (pm.Item.Id == 48)
      {
        RaiseItem(pm, "PowerHerb");
        pm.OnboardPokemon.CoordY = CoordY.Plate;
        return true;
      }
      return false;
    }
    public static double FloatStone(PokemonProxy pm)
    {
      if (pm.Item.Id == 103) return 0.5d;
      return 1d;
    }
    public static bool IronBall(this IItemE item)
    {
      return item.Id == 55;
    }
    public static bool RingTarget(this IItemE item)
    {
      return item.Id == 107;
    }

    #region berry
    public static bool MicleBerry(AtkContext atk)
    {
      PokemonProxy pm = atk.Attacker;
      if (pm.Item.Id == 189 && Abilities.Gluttony(pm))
      {
        RaiseItem(pm, "MicleBerry");
        return true;
      }
      return false;
    }
    #endregion
  }
}
