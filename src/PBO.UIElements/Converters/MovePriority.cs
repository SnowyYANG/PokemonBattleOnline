using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  public class MovePriorityText : Converter<int>
  {
    protected override object Convert(int value)
    {
      if (value > 0) return "优先";
      if (value < 0) return "推迟";
      return string.Empty;
    }
  }
}
