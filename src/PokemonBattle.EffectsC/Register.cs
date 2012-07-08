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
    private static void A(IAbilityE ability)
    {
      GameService.Register(ability);
    }
    private static void I(IItemE item)
    {
      GameService.Register(item);
    }
    private static void M(IMoveE move)
    {
      GameService.Register(move);
    }
    
    public static void Register()
    {
      A(new Illusion(56));

      M(new GustTwister(16));  
      M(new Leap(19, CoordY.Air));//fly
      M(new SpRangeMove(57, CoordY.Water, true));
      M(new GrassKnot(67));
      M(new SolarBeam(76));
      M(new Thunder(87));
      M(new SpRangeMove(89, CoordY.Underground, true));//earthquake
      M(new SpRangeMove(90, CoordY.Underground));
      M(new Leap(91, CoordY.Underground));//dig
      M(new SkullBash(130));
      M(new Flail(175));
      M(new Flail(179));
      M(new AHappy(216));
      M(new BHappy(218));
      M(new HiddenPower(237));
      M(new GustTwister(239));
      M(new Spout(284));
      M(new Leap(291, CoordY.Water));//dives
      M(new WringOut(306));
      M(new Spout(323));
      M(new SpRangeMove(327, CoordY.Air));
      M(new Leap(340, CoordY.Air));//bounce
      M(new GyroBall(360));
      M(new TrumpCard(376));
      M(new WringOut(378));
      M(new Punishment(386));
      M(new GrassKnot(447));
      M(new Leap(467, CoordY.Another));//shadow
      M(new HeavySlam(484));
      M(new ElectroBall(486));
      M(new StoredPower(500));
      M(new Hex(506));
      M(new Acrobatics(512));
      M(new HeavySlam(535));
      M(new Thunder(542));//Hurricane
    }
  }
}
