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
    [ValueConversion(typeof(object), typeof(Visibility))]
    class ObjectVisibilityC : IValueConverter
    {
      public static readonly ObjectVisibilityC I = new ObjectVisibilityC();

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
