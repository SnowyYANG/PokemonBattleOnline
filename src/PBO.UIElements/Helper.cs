using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.PBO
{
  public static class Helper
  {
    public static readonly Random Random = new Random();

    public static SolidColorBrush NewBrush(uint colorcode)
    {
      Color c = new Color();
      c.B = (byte)colorcode;
      colorcode >>= 8;
      c.G = (byte)colorcode;
      colorcode >>= 8;
      c.R = (byte)colorcode;
      colorcode >>= 8;
      c.A = (byte)colorcode;
      SolidColorBrush scb = new SolidColorBrush(c);
      scb.Freeze();
      return scb;
    }
    public static BitmapImage GetImage(string filename)
    {
      return new BitmapImage(new Uri(@"pack://application:,,,/PBO.UIElements;component/images/" + filename, UriKind.Absolute));
    }
    internal static ResourceDictionary GetDictionary(string group, string name)
    {
      return (ResourceDictionary)Application.LoadComponent(
        new Uri(string.Format(@"/PBO.UIElements;component/{0}/{1}.xaml", group, name), UriKind.Relative));
    }
    internal static T GetObject<T>(string group, string filename, string key)
    {
      return (T)GetDictionary(group, filename)[key];
    }
    internal static T GetObject<T>(string group, string name)
    {
      return GetObject<T>(group, name, name);
    }
  }
}
