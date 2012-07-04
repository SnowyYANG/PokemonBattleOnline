using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  static class Paralyzed
  {
    public static bool CanExecute(PokemonProxy p)
    {
      if (p.State == PokemonState.Paralyzed)
      {
        if (p.Controller.OneNth(4))
        {
          p.Controller.ReportBuilder.Add(PositionChange.Reset("ParalyzedWork", p));
          return false;
        }
      }
      return true;
    }
  }
}
