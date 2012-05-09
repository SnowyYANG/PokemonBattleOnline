using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  internal static class Frozen
  {
    public static bool CanExecute(PokemonProxy p)
    {
      if (p.State == PokemonState.Frozen)
      {
        if (p.SelectedMove.Type.AdvancedFlags.AvailableEvenFrozen || p.Controller.GetRandomInt(0, 3) == 0)
          p.State = PokemonState.Normal;
        else
        {
          p.AddReportPm("Frozen");
          return false;
        }
      }
      return true;
    }
  }
}
