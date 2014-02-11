using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static partial class PTs
  {
    public static void ShowLogPm(this PokemonProxy pm, string key, int arg1 = 0, int arg2 = 0)
    {
      pm.Controller.ReportBuilder.ShowLog(key, pm.Id, arg1, arg2);
    }
    public static void NoEffect(this PokemonProxy pm)
    {
      ShowLogPm(pm, "NoEffect");
    }
  }
}
