using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    [ValueConversion(typeof(int?),typeof(Item))]
    class IdItemC : IValueConverter
    {
      public static readonly IdItemC I = new IdItemC();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? intValue = (int?)value;
            if (!intValue.HasValue)
                return null;
            return DataService.GetItem(intValue.Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return (value as Item).Id;
        }
    }
}
