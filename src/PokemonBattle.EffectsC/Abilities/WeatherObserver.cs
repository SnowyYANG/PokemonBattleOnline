using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Abilities
{
  abstract class WeatherObserver : AbilityE
  {
    protected readonly int Number;

    protected WeatherObserver(int id, int number)
      : base(id)
    {
      Number = number;
    }

    protected abstract int GetForm(Weather weather);

    public override void Attach(PokemonProxy pm)
    {
      var form = GetForm(pm.Controller.Weather);
      if (pm.CanChangeForm(Number, form))
      {
        //pm.OnboardPokemon.SetCondition("ObserveWeather"); Sp.Abilities.AttachWeatherObserver
        Raise(pm);
        pm.ChangeForm(form);
      }
    }
    public override void Detach(PokemonProxy pm)
    {
      if (pm.CanChangeForm(Number, 0)) pm.ChangeForm(0);
      pm.OnboardPokemon.RemoveCondition("ObserveWeather");
    }
  }
  class FlowerGift : WeatherObserver
  {
    public FlowerGift(int id)
      : base(id, 421)
    {
    }
    protected override int GetForm(Weather weather)
    {
      return weather == Weather.IntenseSunlight ? 1 : 0;
    }
  }
  class Forecast : WeatherObserver
  {
    public Forecast(int id)
      : base(id, 351)
    {
    }
    protected override int GetForm(Weather weather)
    {
      var form = (int)weather;
      return form > 3 ? 0 : form;
    }
  }
}
