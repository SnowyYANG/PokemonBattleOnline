using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  class BadlyPoison : PmCondition
  {
    public BadlyPoison(PokemonProxy pm)
      : base("BadlyPoison", pm, 15)
    {
    }
  }
}
