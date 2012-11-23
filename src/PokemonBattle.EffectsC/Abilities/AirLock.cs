using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Abilities
{
  class AirLock : AbilityE
  {
    public AirLock(int id)
      : base(id)
    {
    }

    public override void Attach(PokemonProxy pm)
    {
      Raise(pm);
      pm.Controller.ReportBuilder.Add("AirLock");
      if (pm.Controller.Board.Weather != Weather.Normal) Sp.Abilities.WeatherChanged(pm.Controller);
    }
  }
}
