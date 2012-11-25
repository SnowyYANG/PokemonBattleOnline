using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  class AccuracyStringC : Converter<int>
  {
    public static readonly AccuracyStringC I = new AccuracyStringC();

    protected override object Convert(int value)
    {
      return value == 0x65 ? "-" : value.ToString();
    }
  }
  [ValueConversion(typeof(int), typeof(string))]
  class PowerStringC : Converter<int>
  {
    public static readonly PowerStringC I = new PowerStringC();

    protected override object Convert(int value)
    {
      return value == 0 || value == 1 ? "-" : value.ToString();
    }
  }
}
