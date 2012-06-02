using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameService = LightStudio.PokemonBattle.Game.GameService;
using LightStudio.PokemonBattle.Effects.Abilities;
using LightStudio.PokemonBattle.Effects.Conditions;
using LightStudio.PokemonBattle.Effects.Moves;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Effects
{
  public static class EffectsRegister
  {
    public static void Register()
    {
      GameService.Register(new Illusion());
      GameService.Register(new GrassKnot(DataService.GetMoveType(447)));
      GameService.Register(new GrassKnot (DataService.GetMoveType(67)));

    }
  }
}
