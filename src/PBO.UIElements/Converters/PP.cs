using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  public class PPColor : Converter<int>
  {
    public static readonly PPColor C;
    static readonly SolidColorBrush White;
    static readonly SolidColorBrush Yellow; //50%
    static readonly SolidColorBrush Orange; //25%
    static readonly SolidColorBrush Red;
    static PPColor()
    {
      White = Helper.NewBrush(0xfff8f8f8);
      Yellow = Helper.NewBrush(0xfff8d000);
      Orange = Helper.NewBrush(0xfff87000);
      Red = Helper.NewBrush(0xfff80848);
      C = new PPColor();
    }

    protected override object Convert(int value)
    {
      Brush b;
      if (value > 2) b = White;
      else if (value == 2) b = Yellow;
      else if (value == 1) b = Orange;
      else b = Red;
      return b;
    }
  }
  public class PPShadow : Converter<int>
  {
    public static readonly PPShadow C;
    static readonly SolidColorBrush White;
    static readonly SolidColorBrush Yellow; //50%
    static readonly SolidColorBrush Orange; //25%
    static readonly SolidColorBrush Red;
    static PPShadow()
    {
      White = Helper.NewBrush(0xff707070);
      Yellow = Helper.NewBrush(0xff786000);
      Orange = Helper.NewBrush(0xff703800);
      Red = Helper.NewBrush(0xff780830);
      C = new PPShadow();
    }

    protected override object Convert(int value)
    {
      Brush b;
      if (value > 2) b = White;
      else if (value == 2) b = Yellow;
      else if (value == 1) b = Orange;
      else b = Red;
      return b;
    }
  }
}
