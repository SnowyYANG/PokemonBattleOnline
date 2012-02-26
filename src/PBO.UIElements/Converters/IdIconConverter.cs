using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  [ValueConversion(typeof(int), typeof(ImageSource))]
  public class IdIconConverter : Converter<int>
  {
    public static IdIconConverter I = new IdIconConverter();

    protected override object Convert(int value)
    {
      return DataService.Image.GetPokemonIcon((int)value);
    }
  }
}
