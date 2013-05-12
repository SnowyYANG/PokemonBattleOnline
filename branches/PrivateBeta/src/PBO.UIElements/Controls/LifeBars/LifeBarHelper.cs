using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using System.ComponentModel;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  internal static class LifeBarHelper
  {
    public static readonly SolidColorBrush GREEN;
    public static readonly SolidColorBrush GREENSHADOW;
    public static readonly SolidColorBrush YELLOW;
    public static readonly SolidColorBrush YELLOWSHADOW;
    public static readonly SolidColorBrush RED;
    public static readonly SolidColorBrush REDSHADOW;
    
    static LifeBarHelper()
    {
      GREEN = Helper.NewBrush(0xff00f848);
      GREENSHADOW = Helper.NewBrush(0xff008828);
      YELLOW = Helper.NewBrush(0xfff0b000);
      YELLOWSHADOW = Helper.NewBrush(0xff986010);
      RED = Helper.NewBrush(0xfff83040);
      REDSHADOW = Helper.NewBrush(0xff902030);
    }

    public static double GetWidth(int hp, int maxHp)
    {
      int x2 = hp * 48 / maxHp;
      return x2 == 0 && hp != 0 ? 1 : x2;
    }
  }
}
