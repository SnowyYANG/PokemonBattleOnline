using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class Lv7DChanging
  {
    public static int Execute(PokemonProxy pm, PokemonProxy by, StatType stat, int change, bool showFail)
    {
      switch (pm.Ability)
      {
        case As.CLEAR_BODY:
        case As.WHITE_SMOKE:
          if (change < 0 && pm.Pokemon.TeamId != by.Pokemon.TeamId)
          {
            if (showFail)
            {
              pm.RaiseAbility();
              pm.AddReportPm("7DLockAll");
            }
            change = 0;
          }
          break;
        case As.KEEN_EYE:
          change = CantLvDown(pm, by, stat, change, showFail, StatType.Accuracy);
          break;
        case As.HYPER_CUTTER:
          change = CantLvDown(pm, by, stat, change, showFail, StatType.Atk);
          break;
        case As.BIG_PECKS:
          change = CantLvDown(pm, by, stat, change, showFail, StatType.Def);
          break;
        case As.SIMPLE: //86
          change <<= 1;
          break;
        case As.CONTRARY: //126
          change = 0 - change;
          break;
      }
      return change;
    }

    private static int CantLvDown(PokemonProxy pm, PokemonProxy by, StatType stat, int change, bool showFail, StatType s0)
    {
      if (change < 0 && stat == s0 && pm.Pokemon.TeamId != by.Pokemon.TeamId)
      {
        if (showFail)
        {
          pm.RaiseAbility();
          pm.AddReportPm("7DLock", (int)stat);
        }
        change = 0;
      }
      return change;
    }
  }
}
