using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace LightStudio.PokemonBattle.Data
{
  public static class PokemonStatHelper
  {
    public static int Get5D(StatType statType, PokemonNature nature, int typeBase, byte iv, byte ev, byte lv)
    {
      return (((typeBase << 1) + iv + (ev >> 2)) * lv / 100 + 5) * NatureEffects[(int)nature, (int)statType - 1] / 10;
    }
    public static int GetHp(int typeBase, int iv, int ev, int lv)
    {
      return typeBase == 1 ? 1 : (((typeBase << 1) + iv + (ev >> 2)) * lv / 100 + 10 + lv);
    }

    private static readonly sbyte[,] NatureEffects = new sbyte[,]
    {
      { 10, 10, 10, 10, 10 },
      { 11, 9, 10, 10, 10 },
      { 11, 10, 10, 10, 9 },
      { 11, 10, 9, 10, 10 },
      { 11, 10, 10, 9, 10 },
      { 9, 11, 10, 10, 10 },
      { 10, 10, 10, 10, 10 },
      { 10, 11, 10, 10, 9 },
      { 10, 11, 9, 10, 10 },
      { 10, 11, 10, 9, 10 },
      { 9, 10, 10, 10, 11 },
      { 10, 9, 10, 10, 11 },
      { 10, 10, 10, 10, 10 },
      { 10, 10, 9, 10, 11 },
      { 10, 10, 10, 9, 11 },
      { 9, 10, 11, 10, 10 },
      { 10, 9, 11, 10, 10 },
      { 10, 10, 11, 10, 9 },
      { 10, 10, 10, 10, 10 },
      { 10, 10, 11, 9, 10 },
      { 9, 10, 10, 11, 10 },
      { 10, 9, 10, 11, 10 },
      { 10, 10, 10, 11, 9 },
      { 10, 10, 9, 11, 10 },
      { 10, 10, 10, 10, 10 }
    };
  }
}
