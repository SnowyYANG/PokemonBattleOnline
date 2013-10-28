using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Converters
{
  [ValueConversion(typeof(BattleType), typeof(SolidColorBrush))]
  public class BattleTypeBg : IValueConverter
  {
    public readonly static BattleTypeBg C = new BattleTypeBg();
    static readonly SolidColorBrush[] c;
    static BattleTypeBg()
    {
      c = new SolidColorBrush[18];
      c[1] = SBrushes.NewBrush(0xffa8a090);
      c[2] = SBrushes.NewBrush(0xffa05038);
      c[3] = SBrushes.NewBrush(0xff98a8f0);
      c[4] = SBrushes.NewBrush(0xffb058a0);
      c[5] = SBrushes.NewBrush(0xffd0b058);
      c[6] = SBrushes.NewBrush(0xffb8a058);
      c[7] = SBrushes.NewBrush(0xffa8b820);
      c[8] = SBrushes.NewBrush(0xff6060b0);
      c[9] = SBrushes.NewBrush(0xffa8a8c0);
      c[10] = SBrushes.NewBrush(0xfff05030);
      c[11] = SBrushes.NewBrush(0xff3898f8);
      c[12] = SBrushes.NewBrush(0xff78c850);
      c[13] = SBrushes.NewBrush(0xfff8c030);
      c[14] = SBrushes.NewBrush(0xfff870a0);
      c[15] = SBrushes.NewBrush(0xff58c8e0);
      c[16] = SBrushes.NewBrush(0xff7860e0);
      c[17] = SBrushes.NewBrush(0xff705848);
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is BattleType) return c[(int)(byte)value];
      else return null;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
  [ValueConversion(typeof(BattleType), typeof(SolidColorBrush))]
  public class BattleTypeBorder : IValueConverter
  {
    public static readonly BattleTypeBorder C = new BattleTypeBorder();
    static readonly SolidColorBrush[] c;
    static BattleTypeBorder()
    {
      c = new SolidColorBrush[18];
      c[1] = SBrushes.NewBrush(0xff505050);
      c[2] = SBrushes.NewBrush(0xff483830);
      c[3] = SBrushes.NewBrush(0xff405090);
      c[4] = SBrushes.NewBrush(0xff483850);
      c[5] = SBrushes.NewBrush(0xff705018);
      c[6] = SBrushes.NewBrush(0xff705018);
      c[7] = SBrushes.NewBrush(0xff406838);
      c[8] = SBrushes.NewBrush(0xff483850);
      c[9] = SBrushes.NewBrush(0xff505050);
      c[10] = SBrushes.NewBrush(0xff702008);
      c[11] = SBrushes.NewBrush(0xff405090);
      c[12] = SBrushes.NewBrush(0xff406838);
      c[13] = SBrushes.NewBrush(0xff705018);
      c[14] = SBrushes.NewBrush(0xff683838);
      c[15] = SBrushes.NewBrush(0xff405090);
      c[16] = SBrushes.NewBrush(0xff483890);
      c[17] = SBrushes.NewBrush(0xff483830);
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is BattleType) return c[(int)(byte)value];
      else return null;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
  [ValueConversion(typeof(BattleType), typeof(SolidColorBrush))]
  public class BattleTypeCircle : IValueConverter
  {
    public static readonly BattleTypeCircle C = new BattleTypeCircle();
    static readonly SolidColorBrush[] c;
    static BattleTypeCircle()
    {
      c = new SolidColorBrush[18];
      c[1] = SBrushes.NewBrush(0xffD0D0D0);
      c[2] = SBrushes.NewBrush(0xffc05810);
      c[3] = SBrushes.NewBrush(0xff7888f8);
      c[4] = SBrushes.NewBrush(0xff9840c0);
      c[5] = SBrushes.NewBrush(0xffb88818);
      c[6] = SBrushes.NewBrush(0xff907850);
      c[7] = SBrushes.NewBrush(0xff98a018);
      c[8] = SBrushes.NewBrush(0xff585090);
      c[9] = SBrushes.NewBrush(0xffd0b8d0);
      c[10] = SBrushes.NewBrush(0xfff83028);
      c[11] = SBrushes.NewBrush(0xff2078f8);
      c[12] = SBrushes.NewBrush(0xff28b020);
      c[13] = SBrushes.NewBrush(0xfff8c028);
      c[14] = SBrushes.NewBrush(0xfff84068);
      c[15] = SBrushes.NewBrush(0xff48c0f8);
      c[16] = SBrushes.NewBrush(0xff5848f8);
      c[17] = SBrushes.NewBrush(0xff704848);
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is BattleType) return c[(int)(byte)value];
      else return null;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
  public class BattleTypeString : Converter<BattleType>
  {
    public static readonly BattleTypeString C = new BattleTypeString();
    protected override object Convert(BattleType value)
    {
      return GameString.Current.BattleType(value);
    }
  }
}
