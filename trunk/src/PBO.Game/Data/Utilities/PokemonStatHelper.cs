using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace PokemonBattleOnline.Game
{
  public static class PokemonStatHelper
  {
    public static int Get5D(StatType statType, PokemonNature nature, int typeBase, int iv, int ev, int lv)
    {
      return (((typeBase << 1) + iv + (ev >> 2)) * lv / 100 + 5) * nature.StatRevise(statType) / 10;
    }
    public static int GetHp(int typeBase, int iv, int ev, int lv)
    {
      return typeBase == 1 ? 1 : (((typeBase << 1) + iv + (ev >> 2)) * lv / 100 + 10 + lv);
    }
  }
}
