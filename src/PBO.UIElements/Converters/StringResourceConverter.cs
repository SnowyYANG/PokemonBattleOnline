using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using LightStudio.Tactic.Globalization;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  public class StringResourceConverter : IValueConverter
  {
    // Methods
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (((value == null) || (this.Service == null)) || string.IsNullOrEmpty(value.ToString()))
      {
        return null;
      }
      if (string.IsNullOrEmpty(this.Langauge))
      {
        return this.Service[value.ToString()];
      }
      return this.Service.GetString(value.ToString(), this.Langauge);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }

    // Properties
    public string Langauge { get; set; }

    public IDomainStringService Service { get; set; }
  }


}
