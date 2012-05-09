using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameService = LightStudio.PokemonBattle.Game.GameService;
using LightStudio.PokemonBattle.Effects.Abilities;
using LightStudio.PokemonBattle.Effects.Conditions;

namespace LightStudio.PokemonBattle.Effects
{
  public static class EffectsRegister
  {
    public static void Register()
    {
      GameService.Register(new Illusion());
    }
  }
}
