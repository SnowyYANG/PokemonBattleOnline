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
    [ValueConversion(typeof(PokemonType), typeof(IEnumerable<Ability>))]
    class AvailableAbilitiesC : IValueConverter
    {
      public static readonly AvailableAbilitiesC I = new AvailableAbilitiesC();  
      
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (value as PokemonType).GetAvailableAbilities();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
