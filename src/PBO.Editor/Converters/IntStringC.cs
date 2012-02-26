using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  [ValueConversion(typeof(int), typeof(string))]
  class IntStringC : IValueConverter
  {
    public static readonly IntStringC I = new IntStringC();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return null;
      int intValue = (int)value;
      string paramString = string.Format("{0}", parameter).ToLower();
      if (paramString == "accuracy")
      {
        if (intValue == 0)
          return "-";
      }
      else if (paramString == "power")
      {
        if (intValue == 0 || intValue == 1)
          return "-";
      }
      return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
}
