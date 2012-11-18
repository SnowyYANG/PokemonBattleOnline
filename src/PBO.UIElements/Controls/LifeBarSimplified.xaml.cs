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
using System.ComponentModel;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  /// <summary>
  /// Interaction logic for LifeBar.xaml
  /// </summary>
  public partial class LifeBarSimplified : Canvas
  {
    private static void LifeBarSimplified_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      ((LifeBarSimplified)sender).OnDataContextChanged(e);
    }

    private static readonly SolidColorBrush GREEN;
    private static readonly SolidColorBrush GREENSHADOW;
    private static readonly SolidColorBrush YELLOW;
    private static readonly SolidColorBrush YELLOWSHADOW;
    private static readonly SolidColorBrush RED;
    private static readonly SolidColorBrush REDSHADOW;
    
    static LifeBarSimplified()
    {
      GREEN = Helper.NewBrush(0xff60f860);
      GREENSHADOW = Helper.NewBrush(0xff18c020);
      YELLOW = Helper.NewBrush(0xfff8d800);
      YELLOWSHADOW = Helper.NewBrush(0xffe8a800);
      RED = Helper.NewBrush(0xfff87860);
      REDSHADOW = Helper.NewBrush(0xffa83838);
    }

    public LifeBarSimplified()
    {
      InitializeComponent();
      DataContextChanged += LifeBarSimplified_DataContextChanged;
    }

    private void HpChanged(object sender, PropertyChangedEventArgs e)
    {
      var hp = ((PairValue)sender).Value;
      var max = ((PairValue)sender).Origin;
      {
        var x2 = LifeBarHelper.GetWidth(hp, max);
        u.X2 = d.X2 = x2;
      }
      if (hp * 5 <= max)
      {
        u.Stroke = RED;
        d.Stroke = REDSHADOW;
      }
      else if ((hp << 1) <= max)
      {
        u.Stroke = YELLOW;
        d.Stroke = YELLOWSHADOW;
      }
      else
      {
        u.Stroke = GREEN;
        d.Stroke = GREENSHADOW;
      }
    }

    private void OnDataContextChanged(DependencyPropertyChangedEventArgs e)
    {
      if (e.OldValue is PairValue) ((PairValue)e.OldValue).PropertyChanged -= HpChanged;
      if (e.NewValue is PairValue)
      {
        ((PairValue)e.NewValue).PropertyChanged += HpChanged;
        HpChanged(e.NewValue, null);
      }
    }
  }
}
