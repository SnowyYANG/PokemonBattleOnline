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
using System.Windows.Threading;
using System.ComponentModel;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  /// <summary>
  /// Interaction logic for LifeBar.xaml
  /// </summary>
  public partial class LifeBar : Canvas
  {
    static readonly SolidColorBrush GREEN;
    static readonly SolidColorBrush GREENSHADOW;
    static readonly SolidColorBrush YELLOW;
    static readonly SolidColorBrush YELLOWSHADOW;
    static readonly SolidColorBrush RED;
    static readonly SolidColorBrush REDSHADOW;
    static LifeBar()
    {
      GREEN = Helper.NewBrush(0xff00f848);
      GREENSHADOW = Helper.NewBrush(0xff008828);
      YELLOW = Helper.NewBrush(0xfff0b000);
      YELLOWSHADOW = Helper.NewBrush(0xff986010);
      RED = Helper.NewBrush(0xfff83040);
      REDSHADOW = Helper.NewBrush(0xff902030);
    }

    private readonly Storyboard storyboard;
    private readonly DispatcherTimer timer;
    int current;
    int maxHp;
    int hp;
    
    public LifeBar()
    {
      InitializeComponent();
      storyboard = (Storyboard)Resources["Flash"];
      timer = new DispatcherTimer() { Interval = TimeSpan.FromTicks(166666) };
      timer.Tick += timer_Tick;
    }

    private double GetWidth()
    {
      int x2 = current * 48 / maxHp;
      return x2 == 0 && current != 0 ? 1 : x2;
    }
    private void CurrentChanged()
    {
      bar.Width = GetWidth();
      if (current * 5 <= maxHp)
      {
        bar.Background = RED;
        bar.BorderBrush = REDSHADOW;
      }
      else if ((current << 1) <= maxHp)
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

    private void LifeChanged(object sender, PropertyChangedEventArgs e)
    {
      flash.Width = bar.Width;
      hp =((PairValue)sender).Value;
      if (!timer.IsEnabled && current != hp) timer.Start();
    }
    private void timer_Tick(object sender, EventArgs e)
    {
      if (current == hp)
      {
        timer.Stop();
        BeginStoryboard(storyboard);
      }
      else
      {
        if (current < hp) ++current;
        else if (current > hp) --current;
        CurrentChanged();
      }
    }
    private void LifeBar_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (e.OldValue is PairValue) ((PairValue)e.OldValue).PropertyChanged -= LifeChanged;
      PairValue hp = DataContext as PairValue;
      if (hp != null)
      {
        hp.PropertyChanged += LifeChanged;
        maxHp = hp.Origin;
        current = this.hp = hp.Value;
        CurrentChanged();
        flash.Width = GetWidth();
      }
    }
    private void Storyboard_Completed(object sender, EventArgs e)
    {
      flash.Width = GetWidth();
    }
  }
}
