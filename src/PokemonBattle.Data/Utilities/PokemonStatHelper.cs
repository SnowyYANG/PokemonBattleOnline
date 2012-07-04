using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace LightStudio.PokemonBattle.Data
{
  public static class PokemonStatHelper
  {
    #region NatureEffects
    private static readonly double[,] NatureEffects =
        new[,]{
            {1, 1, 1, 1, 1},
            {1.1, 0.9, 1, 1, 1},
            {1.1, 1, 1, 1, 0.9},
            {1.1, 1, 0.9, 1, 1},
            {1.1, 1, 1, 0.9, 1},
            {0.9, 1.1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1.1, 1, 1, 0.9},
            {1, 1.1, 0.9, 1, 1},
            {1, 1.1, 1, 0.9, 1},
            {0.9, 1, 1, 1, 1.1},
            {1, 0.9, 1, 1, 1.1},
            {1, 1, 1, 1, 1},
            {1, 1, 0.9, 1, 1.1},
            {1, 1, 1, 0.9, 1.1},
            {0.9, 1, 1.1, 1, 1},
            {1, 0.9, 1.1, 1, 1},
            {1, 1, 1.1, 1, 0.9},
            {1, 1, 1, 1, 1},
            {1, 1, 1.1, 0.9, 1},
            {0.9, 1, 1, 1.1, 1},
            {1, 0.9, 1, 1.1, 1},
            {1, 1, 1, 1.1, 0.9},
            {1, 1, 0.9, 1.1, 1},
            {1, 1, 1, 1, 1}};
    #endregion

    private static int GetStat(int typeBase, byte iv, byte ev, byte lv, double natureEffect)
    {
      return (int)((((typeBase << 1) + iv + (ev >> 2)) * lv * 0.01 + 5) * natureEffect);
    }
    public static int GetHp(int typeBase, byte iv, byte ev, byte lv)
    {
      return (int)(((typeBase << 1) + iv + (ev >> 2)) * lv * 0.01 + 10 + lv);
    }
    public static int GetStat(StatType statType, PokemonNature nature, int typeBase, byte iv, byte ev, byte lv)
    {
      if (statType == StatType.Hp)
      {
        return GetHp(typeBase, iv, ev, lv);
      }
      else
      {
        return GetStat(typeBase, iv, ev, lv, GetNatureEffect(statType, nature));
      }
    }
    public static double GetNatureEffect(StatType statType, PokemonNature nature)
    {
      return NatureEffects[(int)nature, (int)statType - 1];
    }

    #region HiddenPower

    #region HiddenTypes
    private static readonly BattleType[] hiddenTypes = new[] { 
      BattleType.Fighting, 
      BattleType.Flying,
      BattleType.Poison,
      BattleType.Ground,
      BattleType.Rock,
      BattleType.Bug,
      BattleType.Ghost,
      BattleType.Steel,
      BattleType.Fire,
      BattleType.Water,
      BattleType.Grass,
      BattleType.Electric,
      BattleType.Psychic,
      BattleType.Ice,
      BattleType.Dragon,
      BattleType.Dark};
    public static IEnumerable<BattleType> HiddenPowerTypes
    { get { return hiddenTypes; } }
    #endregion

    #region Built-in Hidden Power IV set
    private static readonly byte[][] IvSet = new byte[][] {
      new byte[]{31, 31, 30, 30, 30, 30},//fighting
      new byte[]{31, 31, 30, 30, 30, 30},//flying
      new byte[]{31, 31, 30, 31, 30, 30},//poison
      new byte[]{30, 30, 30, 30, 31, 30},//ground
      new byte[]{31, 31, 30, 30, 31, 30},//rock
      new byte[]{31, 31, 31, 30, 31, 30},//bug
      new byte[]{31, 31, 30, 31, 31, 30},//ghost
      new byte[]{31, 31, 31, 31, 31, 30},//steel
      new byte[]{31, 31, 30, 30, 30, 31},//fire
      new byte[]{31, 31, 31, 30, 30, 31},//water
      new byte[]{31, 31, 30, 31, 30, 31},//grass
      new byte[]{31, 31, 31, 31, 30, 31},//electric
      new byte[]{31, 31, 30, 30, 31, 31},//psychic
      new byte[]{31, 31, 31, 30, 31, 31},//ice
      new byte[]{31, 31, 30, 31, 31, 31},//dragon
      new byte[]{31, 31, 31, 31, 31, 31},//dark
    };
    #endregion

    /// <summary>
    /// order of Iv : hp, atk, def, speed, spatk, spdef
    /// </summary>
    public static BattleType GetHiddenPowerBattleType(params byte[] statIvs)
    {
      int value = GetHiddenPowerBinaryValue(0, statIvs);
      return hiddenTypes[value * 15 / 63];
    }

    /// <summary>
    /// order of Iv : hp, atk, def, speed, spatk, spdef
    /// </summary>
    public static int GetHiddenPowerPower(params byte[] statIvs)
    {
      int value = GetHiddenPowerBinaryValue(1, statIvs);
      return value * 40 / 63 + 30;
    }

    /// <summary>
    /// order of Iv : hp, atk, def, speed, spatk, spdef
    /// </summary>
    private static int GetHiddenPowerBinaryValue(int bitIndex, byte[] statIVs)
    {
      Contract.Requires(statIVs.Length == 6);

      int[] bits = new int[6];
      for (int i = 0; i < 6; i++)
        bits[i] = (statIVs[i] >> bitIndex) & 1;

      int value = 0;
      for (int i = 5; i >= 0; i--)
        value = (value << 1) + bits[i];
      return value;
    }

    /// <summary>
    /// get a Iv set eligible for the specified Hidden Power type
    /// </summary>
    public static byte[] GetHiddenPowerIvSet(BattleType type)
    {
      Contract.Requires(type != BattleType.Invalid && type != BattleType.Normal);

      return IvSet[(int)type - 2];
    }
    #endregion
  }
}
