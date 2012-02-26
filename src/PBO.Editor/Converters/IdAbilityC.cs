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
    [ValueConversion(typeof(int),typeof(Ability))]
    class IdAbilityC : IValueConverter
    {
      public static readonly IdAbilityC I = new IdAbilityC();  
      
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return DataService.GetAbility((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (value as Ability).Id;
        }
    }
}
