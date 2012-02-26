using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  [ValueConversion(typeof(BattleType), typeof(SolidColorBrush))]
  public class BattleTypeBg : IValueConverter
  {
    public readonly static BattleTypeBg I = new BattleTypeBg();
    static readonly SolidColorBrush[] c;
    static BattleTypeBg()
    {
      c = new SolidColorBrush[18];
      c[1] = Helper.NewBrush(0xffa8b820);
      c[2] = Helper.NewBrush(0xff705848);
      c[3] = Helper.NewBrush(0xff7860e0);
      c[4] = Helper.NewBrush(0xfff8c030);
      c[5] = Helper.NewBrush(0xffa05038);
      c[6] = Helper.NewBrush(0xfff05030);
      c[7] = Helper.NewBrush(0xff98a8f0);
      c[8] = Helper.NewBrush(0xff6060b0);
      c[9] = Helper.NewBrush(0xff78c850);
      c[10] = Helper.NewBrush(0xffd0b058);
      c[11] = Helper.NewBrush(0xff58c8e0);
      c[12] = Helper.NewBrush(0xffa8a090);
      c[13] = Helper.NewBrush(0xffb058a0);
      c[14] = Helper.NewBrush(0xfff870a0);
      c[15] = Helper.NewBrush(0xffb8a058);
      c[16] = Helper.NewBrush(0xffa8a8c0);
      c[17] = Helper.NewBrush(0xff3898f8);
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
    public static readonly BattleTypeBorder I = new BattleTypeBorder();
    static readonly SolidColorBrush[] c;
    static BattleTypeBorder()
    {
      c = new SolidColorBrush[18];
      c[1] = Helper.NewBrush(0xff406838);
      c[2] = Helper.NewBrush(0xff483830);
      c[3] = Helper.NewBrush(0xff483890);
      c[4] = Helper.NewBrush(0xff705018);
      c[5] = Helper.NewBrush(0xff483830);
      c[6] = Helper.NewBrush(0xff702008);
      c[7] = Helper.NewBrush(0xff405090);
      c[8] = Helper.NewBrush(0xff483850);
      c[9] = Helper.NewBrush(0xff406838);
      c[10] = Helper.NewBrush(0xff705018);
      c[11] = Helper.NewBrush(0xff405090);
      c[12] = Helper.NewBrush(0xff505050);
      c[13] = Helper.NewBrush(0xff483850);
      c[14] = Helper.NewBrush(0xff683838);
      c[15] = Helper.NewBrush(0xff705018);
      c[16] = Helper.NewBrush(0xff505050);
      c[17] = Helper.NewBrush(0xff405090);
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
    public static readonly BattleTypeCircle I = new BattleTypeCircle();
    static readonly SolidColorBrush[] c;
    static BattleTypeCircle()
    {
      c = new SolidColorBrush[18];
      c[1] = Helper.NewBrush(0xff98a018);
      c[2] = Helper.NewBrush(0xff704848);
      c[3] = Helper.NewBrush(0xff5848f8);
      c[4] = Helper.NewBrush(0xfff8c028);
      c[5] = Helper.NewBrush(0xffc05810);
      c[6] = Helper.NewBrush(0xfff83028);
      c[7] = Helper.NewBrush(0xff7888f8);
      c[8] = Helper.NewBrush(0xff585090);
      c[9] = Helper.NewBrush(0xff28b020);
      c[10] = Helper.NewBrush(0xffb88818);
      c[11] = Helper.NewBrush(0xff48c0f8);
      c[12] = Helper.NewBrush(0xffD0D0D0);
      c[13] = Helper.NewBrush(0xff9840c0);
      c[14] = Helper.NewBrush(0xfff84068);
      c[15] = Helper.NewBrush(0xff907850);
      c[16] = Helper.NewBrush(0xffd0b8d0);
      c[17] = Helper.NewBrush(0xff2078f8);
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
}
