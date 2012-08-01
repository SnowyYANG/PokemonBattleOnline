using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class WeatherChange : GameEvent
  {
    [DataMember]
    Weather Weather;

    public WeatherChange(Weather weather)
    {
      Weather = weather;
    }

    Weather oldWeather;
    public override void Update(Game.GameOutward game)
    {
      base.Update(game);
      oldWeather = game.Board.Weather;
      game.Board.Weather = Weather;
    }
    public override IText GetGameLog()
    {
      return GetGameLog(Weather == Weather.Normal ? "De" + oldWeather : "En" + Weather);
    }
  }
}
