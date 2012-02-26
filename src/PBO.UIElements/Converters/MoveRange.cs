using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  [ValueConversion(typeof(MoveRange), typeof(string))]
  public class MoveRangeText : Converter<MoveRange>
  {
    protected override object Convert(MoveRange value)
    {
      return value.ToString();
    }
  }
}
