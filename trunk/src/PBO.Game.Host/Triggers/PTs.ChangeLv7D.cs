using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
  internal static partial class PTs
  {
    private static void ChangeLv7DImplement(this PokemonProxy pm, PokemonProxy by, StatType stat, int actualChange, string log)
    {
      if (actualChange != 0)
      {
        if (stat == StatType.Accuracy) pm.OnboardPokemon.AccuracyLv += actualChange;
        else if (stat == StatType.Evasion) pm.OnboardPokemon.EvasionLv += actualChange;
        else pm.OnboardPokemon.ChangeLv7D(stat, actualChange);
        if (log == null)
          switch (actualChange)
          {
            case 1:
              log = "7DUp1";
              break;
            case 2:
              log = "7DUp2";
              break;
            case -1:
              log = "7DDown1";
              break;
            case -2:
              log = "7DDown2";
              break;
            default:
              if (actualChange > 0) log = "7DUp3";
              else log = "7DDown3";
              break;
          }
        ShowLogPm(pm, log, (int)stat);
        if ((by == null || by.Pokemon.TeamId != pm.Pokemon.TeamId) && actualChange < 0) STs.Lv7DDown(pm);
      }
    }
    /// <summary>
    /// null log to show default log
    /// </summary>
    /// <param name="by"></param>
    /// <param name="stat"></param>
    /// <param name="change"></param>
    /// <param name="showFail"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    public static bool ChangeLv7D(this PokemonProxy pm, PokemonProxy by, StatType stat, int change, bool showFail, bool ability = false, string log = null)
    {
      change = CanChangeLv7D(pm, by, stat, change, showFail);
      if (change != 0)
      {
        if (ability) ATs.RaiseAbility(pm);
        ChangeLv7DImplement(pm, by, stat, change, log);
        ITs.WhiteHerb(pm);
        return true;
      }
      return false;
    }
    public static bool ChangeLv7D(this PokemonProxy pm, PokemonProxy by, bool showFail, bool ability, int a, int d = 0, int sa = 0, int sd = 0, int s = 0, int ac = 0, int e = 0)
    {
      a = CanChangeLv7D(pm, by, StatType.Atk, a, false);
      d = CanChangeLv7D(pm, by, StatType.Def, d, false);
      sa = CanChangeLv7D(pm, by, StatType.SpAtk, sa, false);
      sd = CanChangeLv7D(pm, by, StatType.SpDef, sd, false);
      s = CanChangeLv7D(pm, by, StatType.Speed, s, false);
      ac = CanChangeLv7D(pm, by, StatType.Accuracy, ac, false);
      e = CanChangeLv7D(pm, by, StatType.Evasion, e, false);
      if (a != 0 || d != 0 || sa != 0 || sd != 0 || s != 0 || ac != 0 || e != 0)
      {
        if (ability) ATs.RaiseAbility(pm);
        ChangeLv7DImplement(pm, by, StatType.Atk, a, null);
        ChangeLv7DImplement(pm, by, StatType.SpAtk, sa, null);
        ChangeLv7DImplement(pm, by, StatType.Def, d, null);
        ChangeLv7DImplement(pm, by, StatType.SpDef, sd, null);
        ChangeLv7DImplement(pm, by, StatType.Speed, s, null);
        ChangeLv7DImplement(pm, by, StatType.Accuracy, ac, null);
        ChangeLv7DImplement(pm, by, StatType.Evasion, e, null);
        ITs.WhiteHerb(pm);
        return true;
      }
      return false;
    }
    public static bool ChangeLv7D(this PokemonProxy pm, PokemonProxy by, MoveType move)
    {
      bool r = false;
      var c0 = move.Lv7DChanges.FirstOrDefault();
      if (c0 != null)
      {
        bool showFail = move.Category == MoveCategory.Status;
        if (c0.Type == StatType.All) r = ChangeLv7D(pm, by, showFail, false, c0.Change, c0.Change, c0.Change, c0.Change, c0.Change);
        else
        {
          foreach (MoveLv7DChange c in move.Lv7DChanges)
          {
            var ac = CanChangeLv7D(pm, by, c.Type, c.Change, showFail);
            if (ac != 0)
            {
              ChangeLv7DImplement(pm, by, c.Type, ac, null);
              r = true;
            }
          }
          if (r) ITs.WhiteHerb(pm);
        }
      }
      return r;
    }
    public static bool ChangeLv7D(this PokemonProxy pm, DefContext def)
    {
      var c = def.AtkContext.Move.Lv7DChanges.FirstOrDefault();
      return c != null && def.RandomHappen(c.Probability) && ChangeLv7D(pm, def.AtkContext.Attacker, def.AtkContext.Move);
    }
  }
}
