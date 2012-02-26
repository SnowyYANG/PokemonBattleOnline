using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  static class GameService
  {
    public static Rule GetRule(int id)
    {
      return new Rule(id);
    }
  }
}
