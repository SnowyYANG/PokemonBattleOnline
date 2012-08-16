using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  public interface IMoveE
  {
    MoveType Move { get; }
    void Execute(PokemonProxy pm, GameEvents.UseMove eventForPP);
  }
  public static partial class EffectsService
  {
    private sealed class Move0 : IMoveE
    {
      private readonly int Id;
      
      public Move0(int id)
      {
        Id = id;
        Move = DataService.GetMove(id);
      }

      public MoveType Move
      { get; private set; }



      public void Execute(PokemonProxy pm, GameEvents.UseMove eventForPP)
      {
        pm.Controller.ReportBuilder.Add("unfinish", pm, Move == null ? Id.ToString() : Move.GetLocalizedName());
        pm.Action = PokemonAction.Done;
      }
    }
  }
}