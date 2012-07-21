using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Sp.Conditions
{
  public static class Imprison
  {
    internal static bool CanExecute(PokemonProxy p)
    {
      MoveType move = p.SelectedMove.Type;
      foreach (PokemonProxy pm in p.Controller.OnboardPokemons)
        if (pm.OnboardPokemon.HasCondition("Imprison"))
          foreach (MoveProxy m in pm.Moves)
            if (m.Type == move)
            {
              p.Controller.ReportBuilder.Add(
                GameEvents.PositionChange.Reset("Imprison", p, move.GetLocalizedName()));
              return false;
            }
      return true;
    }
  }
}
