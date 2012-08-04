using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class WeatherChange : GameEvent
  {
    [DataMember]
    Weather Weather;

    public WeatherChange(Weather weather)
    {
      Weather = weather;
    }

    protected override void Update()
    {
      AppendGameLog(Weather == Weather.Normal ? "De" + Game.Board.Weather : "En" + Weather);
      Game.Board.Weather = Weather;
    }
  }
}
