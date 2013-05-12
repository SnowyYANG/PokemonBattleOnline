using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Globalization;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    class BoolVisibilityC : IValueConverter
    {
      public static readonly BoolVisibilityC I = new BoolVisibilityC();  
      
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            bool boolValue = (bool)value;
            if (string.Format("{0}", parameter).Equals("inverse", StringComparison.OrdinalIgnoreCase))
                boolValue = !boolValue;

            if (boolValue)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
