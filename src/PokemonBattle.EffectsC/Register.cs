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

        GameService.Register(new GrassKnot(447));
        GameService.Register(new GrassKnot(67));
        GameService.Register(new SpRangeMove(87, CoordY.Air));
        GameService.Register(new SpRangeMove(327, CoordY.Air));
        GameService.Register(new SpRangeMove(542, CoordY.Air));
        GameService.Register(new SpRangeMove(479, CoordY.Air));
        GameService.Register(new Leap(340, CoordY.Air));//bounce
        GameService.Register(new Leap(19, CoordY.Air));//fly
        GameService.Register(new Leap(291, CoordY.Water));//dives
        GameService.Register(new Leap(91, CoordY.Underground));//dig
        GameService.Register(new Leap(467, CoordY.Another));//shadow
        GameService.Register(new SolarBeam());
        GameService.Register(new SkullBash());
    }
  }
}
