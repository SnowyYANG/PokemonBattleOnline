using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  static class Paralyzed
  {
    public static bool CanExecute(PokemonProxy p)
    {
      if (p.State == PokemonState.Paralyzed)
      {
        p.AddReportPm("Paralyzed");
        if (p.Controller.OneNth(4))
        {
          p.AddReportPm("ParalyzedWork");
          return false;
        }
      }
      return true;
    }
  }
}
