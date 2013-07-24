using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;

namespace LightStudio.PokemonBattle.Game.Host.Triggers
{
  internal static class AbilityDetach
  {
    public static void Execute(PokemonProxy pm)
    {
      switch (pm.Ability)
      {
        case As.ZEN_MODE:
          if (pm.CanChangeForm(555, 0)) pm.ChangeForm(0);
          break;
        case As.ILLUSION:
          As.DeIllusion(pm);
          break;
        case As.FLOWER_GIFT:
          WeatherObserver(pm, 421);
          break;
        case As.FORECAST:
          WeatherObserver(pm, 351);
          break;
      }
    }
    private static void WeatherObserver(PokemonProxy pm, int number)
    {
      if (pm.CanChangeForm(number, 0)) pm.ChangeForm(0);
      pm.OnboardPokemon.RemoveCondition("ObserveWeather");
    }
  }
}
