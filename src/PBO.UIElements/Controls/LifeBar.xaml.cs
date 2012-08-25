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
  public partial class LifeBar : Border
  {
    const double YELLOWBAR_MAXLENGTH = 24;
    const double REDBAR_MAXLENGTH = 10;
    static readonly SolidColorBrush GREEN;
    static readonly SolidColorBrush GREENSHADOW;
    static readonly SolidColorBrush YELLOW;
    static readonly SolidColorBrush YELLOWSHADOW;
    static readonly SolidColorBrush RED;
    static readonly SolidColorBrush REDSHADOW;
    static LifeBar()
    {
      GREEN = Helper.NewBrush(0xff00f848);
      GREENSHADOW = Helper.NewBrush(0xff00b820);
      YELLOW = Helper.NewBrush(0xfff8a800);
      YELLOWSHADOW = Helper.NewBrush(0xffa08028);
      RED = Helper.NewBrush(0xfff84070);
      REDSHADOW = Helper.NewBrush(0xffa04858);
    }

    DoubleAnimation da;
    public LifeBar()
    {
      InitializeComponent();
      da = new DoubleAnimation();
      da.Completed += new EventHandler(da_Completed);
    }

    private void LifeBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (bar.Width <= REDBAR_MAXLENGTH)
      {
        bar.Background = RED;
        bar.BorderBrush = REDSHADOW;
      }
      else if (bar.Width <= YELLOWBAR_MAXLENGTH)
      {
        bar.Background = YELLOW;
        bar.BorderBrush = YELLOWSHADOW;
      }
      else
      {
        bar.Background = GREEN;
        bar.BorderBrush = GREENSHADOW;
      }
    }

    private void LifeBar_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (e.OldValue is PairValue)
        ((PairValue)e.OldValue).PropertyChanged -= LifeChanged;
      PairValue hp = DataContext as PairValue;
      if (hp != null)
      {
        bar.BeginAnimation(Border.WidthProperty, null);
        bar.Width = hp.NormalizedValue;
        hp.PropertyChanged += LifeChanged;
        da.SpeedRatio = hp.Origin / 200.0;
      }
    }

    private void LifeChanged(object sender, PropertyChangedEventArgs e)
    {
      flash.Width = bar.Width;
      da.To = ((PairValue)sender).NormalizedValue;
      bar.BeginAnimation(Border.WidthProperty, da);
    }

    void da_Completed(object sender, EventArgs e)
    {
      LifeBar_SizeChanged(null, null);
      if (flash.Width > bar.Width)
        BeginStoryboard((Storyboard)Resources["Flash"]);
    }
  }
}
