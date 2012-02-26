using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  public abstract class Converter<T> : IValueConverter
  {
    //Unfortunately T and converter is not 1 to 1
    //internal readonly static T Instance;
    //static Converter()
    //{
    //  Instance = new T();
    //}

    protected abstract object Convert(T value);

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is T) return Convert((T)value);
      return null;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
}
