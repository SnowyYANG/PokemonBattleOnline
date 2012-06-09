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
    private static void M(IMoveE move)
    {
      GameService.Register(move);
    }
    
    public static void Register()
    {
        GameService.Register(new Illusion());

        M(new GrassKnot(447));
        M(new GrassKnot(67));
        M(new SpRangeMove(87, CoordY.Air));
        M(new SpRangeMove(327, CoordY.Air));
        M(new SpRangeMove(542, CoordY.Air));
        M(new SpRangeMove(479, CoordY.Air));
        M(new SpRangeMove(57, CoordY.Water, true));
        M(new SpRangeMove(89, CoordY.Underground, true));
        M(new SpRangeMove(90, CoordY.Underground));
        M(new Leap(340, CoordY.Air));//bounce
        M(new Leap(19, CoordY.Air));//fly
        M(new Leap(291, CoordY.Water));//dives
        M(new Leap(91, CoordY.Underground));//dig
        M(new Leap(467, CoordY.Another));//shadow
        M(new SolarBeam());
        M(new SkullBash());
    }
  }
}
