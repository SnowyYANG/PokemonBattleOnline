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
  class HiddenPowerC : IMultiValueConverter
  {
    public static readonly HiddenPowerC I = new HiddenPowerC();
    
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values.All(value => value is byte)) return PokemonStatHelper.GetHiddenPowerBattleType(values.Cast<byte>().ToArray());
      return null;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      if (value is BattleType) return PokemonStatHelper.GetHiddenPowerIvSet((BattleType)value).Cast<object>().ToArray();
      return null;
    }
  }
}
