using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
  public static class LearnList
  {
    public static GenLearnList[] Raw;

    public static PokemonLearnList GetLearnList(int number, int gen)
    {
      return Raw[gen - 3].GetLearnList(number);
    }
  }
}
