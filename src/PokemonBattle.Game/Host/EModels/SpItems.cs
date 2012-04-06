using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  internal static class SpItems
  {
    #region ids
    private const int WHITE_HERB = 5;
    private const int MICLE_BERRY = 189;
    #endregion

    public static void RaiseItem(this PokemonProxy pm)
    {
      pm.Item.Raise(pm);
    }

    public static void CheckWhiteHerb(PokemonProxy pm)
    {
      if (pm.Item.Id == WHITE_HERB)
        pm.RaiseItem();
    }
    public static bool CheckMicleBerry(AtkContext atk)
    {
      if (atk.Attacker.Item.Id == MICLE_BERRY)
      {
        atk.Attacker.Item.Raise(atk.Attacker);
        return true;
      }
      return false;
    }
  }
}
