using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Sp
{
  internal static class Items
  {
    #region ids
    const int RED_CARD = 106;
    const int EJECT_BUTTON = 111;
    #endregion

    private static void RaiseItem(this PokemonProxy pm, string key = "RaiseItem")
    {
      pm.Controller.ReportBuilder.Add(new Interactive.GameEvents.UseItem(key, pm, pm.Pokemon.Item));
      if (pm.Pokemon.Item.Type != ItemType.Normal) pm.ConsumeItem();
    }
    public static void RaiseItem(this PokemonProxy pm)
    {
      pm.Item.Raise(pm);
    }

    public static bool BigRoot(this IItemE item)
    {
      return item.Id == 73;
    }

    public static void CheckWhiteHerb(PokemonProxy pm)
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
        if (raise) RaiseItem(pm);
      }
    }
    public static bool CheckMicleBerry(AtkContext atk)
    {
      PokemonProxy pm = atk.Attacker;
      if (pm.Item.Id == 189 && pm.Pokemon.Hp.Percentage < (pm.Ability.Gluttony()? 0.5 : 0.25))
      {
        RaiseItem(pm);
        return true;
      }
      return false;
    }
    public static void WideLens(AtkContext atk)
    {
      if (atk.Attacker.Item.Id == 42)
        atk.AccuracyModifier *= 0x1199;
    }
    public static Modifier ZoomLens(DefContext target)
    {
      if (target.AtkContext.Attacker.Item.Id == 53) ;
      return 0x1000;
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
      const int LIFE_ORB = 47;

      PokemonProxy aer = target.AtkContext.Attacker;
      int item = aer.Item.Id;
      ushort r = 0x1000;
      if (target.EffectRevise > 0 && aer.Item.Id == EXPERT_BELT)
        r = 0x1333;
      return r;
    }
    /// <summary>
    /// 气球的提示信息不是Attach而是Debut，是唯一会Debut的道具
    /// </summary>
    public static void AirBalloon(PokemonProxy pm)
    {
      if (pm.Item.Id == 105)
        pm.AddReportPm("EnBalloon");
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
      if ((pm.Item.Id == FOCUS_BAND && !pm.Pokemon.Hp.IsChanged) ||
        (pm.Item.Id == FOCUS_SASH && pm.Controller.OneNth(10)))
      {
        pm.RaiseItem();
        return true;
      }
      return false;
    }
    public static void AttackPostEffect(AtkContext atk)
    {
      const int SHELL_BELL = 35, LIFE_ORB = 47;
      if (atk.Attacker.Item.Id == SHELL_BELL)
      {
      }
      else if (atk.Attacker.Item.Id == LIFE_ORB)
      {
      }
    }
    public static bool CanAttackFlinch(DefContext def)
    {
      const int KINGS_ROCK = 10, RAZOR_FANG = 97;
      int item = def.AtkContext.Attacker.Item.Id;
      return (item == KINGS_ROCK || item == RAZOR_FANG) && def.Defender.Controller.GetRandomInt(0, 9) == 0;
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
  }
}
