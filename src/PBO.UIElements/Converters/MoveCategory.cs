using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  [ValueConversion(typeof(MoveCategory), typeof(string))]
  public class MoveCategoryText : Converter<MoveCategory>
  {
    protected override object Convert(MoveCategory value)
    {
      return DataService.String[value.ToString()];
    }
  }
}
