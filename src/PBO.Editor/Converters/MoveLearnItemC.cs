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
    [ValueConversion(typeof(int), typeof(MoveLearnItemViewModel))]
    class MoveLearnItemC : IValueConverter
    {
      public static readonly MoveLearnItemC I = new MoveLearnItemC();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (Editor.CurrentEditor.EditingPokemon.Learnset != null)
            {
                return Editor.CurrentEditor.EditingPokemon.Learnset.
                    FirstOrDefault(m => m.MoveType.Id == (int)value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            return null;
        }
    }
}
