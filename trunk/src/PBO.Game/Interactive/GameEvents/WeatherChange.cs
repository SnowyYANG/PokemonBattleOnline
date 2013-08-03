using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class WeatherChange : GameEvent
  {
    [DataMember(Name = "a")]
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
