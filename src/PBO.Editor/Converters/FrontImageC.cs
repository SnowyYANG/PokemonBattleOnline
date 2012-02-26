using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    public class FrontImageC : IMultiValueConverter
    {
      public static readonly FrontImageC I = new FrontImageC();
      static readonly ImageService imageService = DataService.Image;

        public object Convert(object[] values, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is int && values[1] is PokemonGender)
            {
                int id = (int)values[0];
                PokemonGender gender = (PokemonGender)values[1];
                if (gender == PokemonGender.Female)
                {
                    return imageService.GetPokemonFemaleFront(id);
                }
                else
                {
                    return imageService.GetPokemonMaleFront(id);
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, 
            CultureInfo culture)
        {
            return null;
        }
    }
}
