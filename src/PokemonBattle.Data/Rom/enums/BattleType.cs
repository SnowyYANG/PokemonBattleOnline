﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
  public enum BattleType : byte
  {
    Invalid,
    Normal,
    Fighting,
    Flying,
    Poison,
    Ground,
    Rock,
    Bug,
    Ghost,
    Steel,
    Fire,
    Water,
    Grass,
    Electric,
    Psychic,
    Ice,
    Dragon,
    Dark
  }
  public static class BattleTypeHelper
  {
    /// <summary>
    /// [atk, def]
    /// </summary>
    private static readonly sbyte[,] REVISE = new sbyte[18, 18] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 1, 0, -1, -1, 0, 1, -1, 0, 1, 0, 0, 0, 0, -1, 1, 0, 1 }, { 0, 0, 1, 0, 0, 0, -1, 1, 0, -1, 0, 0, 1, -1, 0, 0, 0, 0 }, { 0, 0, 0, 0, -1, -1, -1, 0, -1, 0, 0, 0, 1, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 1, 0, 1, -1, 0, 1, 1, 0, -1, 1, 0, 0, 0, 0 }, { 0, 0, -1, 1, 0, -1, 0, 1, 0, -1, 1, 0, 0, 0, 0, 1, 0, 0 }, { 0, 0, -1, -1, -1, 0, 0, 0, -1, -1, -1, 0, 1, 0, 1, 0, 0, 1 }, { 0, 0, 0, 0, 0, 0, 0, 0, 1, -1, 0, 0, 0, 0, 1, 0, 0, -1 }, { 0, 0, 0, 0, 0, 0, 1, 0, 0, -1, -1, -1, 0, -1, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0, 0, -1, 1, 0, 1, -1, -1, 1, 0, 0, 1, -1, 0 }, { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, -1, -1, 0, 0, 0, -1, 0 }, { 0, 0, 0, -1, -1, 1, 1, -1, 0, -1, -1, 1, -1, 0, 0, 0, -1, 0 }, { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, -1, -1, 0, 0, -1, 0 }, { 0, 0, 1, 0, 1, 0, 0, 0, 0, -1, 0, 0, 0, 0, -1, 0, 0, 0 }, { 0, 0, 0, 1, 0, 1, 0, 0, 0, -1, -1, -1, 1, 0, 0, -1, 1, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, 1, 0 }, { 0, 0, -1, 0, 0, 0, 0, 0, 1, -1, 0, 0, 0, 0, 1, 0, 0, -1 } };
    /// <summary>
    /// [atk]
    /// </summary>
    private static readonly BattleType[] NO_EFFECT;

    static BattleTypeHelper()
    {
      NO_EFFECT = new BattleType[18];
      NO_EFFECT[(int)BattleType.Normal] = NO_EFFECT[(int)BattleType.Fighting] = BattleType.Ghost;
      NO_EFFECT[(int)BattleType.Electric] = BattleType.Ground;
      NO_EFFECT[(int)BattleType.Poison] = BattleType.Steel;
      NO_EFFECT[(int)BattleType.Psychic] = BattleType.Dark;
      NO_EFFECT[(int)BattleType.Ghost] = BattleType.Normal;
      NO_EFFECT[(int)BattleType.Ground] = BattleType.Flying;
    }

    public static bool NoEffect(this BattleType a, BattleType d)
    {
      return d != BattleType.Invalid && NO_EFFECT[(int)a] == d;
    }
    public static BattleType NoEffect(this BattleType a)
    {
      return NO_EFFECT[(int)a];
    }
    public static int EffectRevise(this BattleType a, BattleType d)
    {
      return REVISE[(int)a, (int)d];
    }
    public static int EffectRevise(this BattleType a, BattleType d1, BattleType d2)
    {
      return EffectRevise(a, d1) + EffectRevise(a, d2);
    }
  }
}
