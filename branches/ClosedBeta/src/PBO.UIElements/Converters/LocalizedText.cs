using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

namespace PokemonBattleOnline.PBO.Converters
{
  public class LocalizedText : IValueConverter
  {
    public static readonly LocalizedText C = new LocalizedText();
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value == null ? null : Data.DataService.String[value.ToString()];
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
}
