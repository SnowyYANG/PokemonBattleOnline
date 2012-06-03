using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameService = LightStudio.PokemonBattle.Game.GameService;
using LightStudio.PokemonBattle.Effects.Abilities;
using LightStudio.PokemonBattle.Effects.Conditions;
using LightStudio.PokemonBattle.Effects.Moves;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Effects
{
  public static class EffectsRegister
  {
    public static void Register()
    {
        GameService.Register(new Illusion());
        GameService.Register(new GrassKnot(DataService.GetMoveType(447)));
        GameService.Register(new GrassKnot(DataService.GetMoveType(67)));
        GameService.Register(new SpRangeMove(DataService.GetMoveType(87), CoordY.Air));
        GameService.Register(new SpRangeMove(DataService.GetMoveType(327), CoordY.Air));
        GameService.Register(new SpRangeMove(DataService.GetMoveType(542), CoordY.Air));
        GameService.Register(new SpRangeMove(DataService.GetMoveType(479), CoordY.Air));
        
    }
  }
}
