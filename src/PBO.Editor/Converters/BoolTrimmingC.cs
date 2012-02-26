using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Globalization;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  [ValueConversion(typeof(bool), typeof(TextTrimming))]
  class BoolTrimmingC : IValueConverter
  {
    public static readonly BoolTrimmingC I = new BoolTrimmingC();

    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
      if (value == null)
        return null;
      bool boolValue = (bool)value;
      if (string.Format("{0}", parameter).Equals("inverse", StringComparison.OrdinalIgnoreCase))
      {
        boolValue = !boolValue;
      }
      if (boolValue)
      {
        return TextTrimming.CharacterEllipsis;
      }
      else
      {
        return TextTrimming.None;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
      return null;
    }
  }
}
