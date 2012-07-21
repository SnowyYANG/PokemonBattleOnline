using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game.GameEvents;

namespace LightStudio.PokemonBattle.Game.Host.Sp.Conditions
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
