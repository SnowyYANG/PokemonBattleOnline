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
  /// <summary>
  /// Interaction logic for LifeBar.xaml
  /// </summary>
  public partial class LifeBar : Canvas
  {
    private const int PERIOD = 16;
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
    private static void TimerCallback(object o)
    {
      ((LifeBar)o).TimerCallback();
    }

    private readonly Storyboard storyboard;
    private readonly Timer timer;
    
    public LifeBar()
    {
      InitializeComponent();
      storyboard = (Storyboard)Resources["Flash"];
      timer = new Timer(TimerCallback, this, Timeout.Infinite, PERIOD);
    }

    int maxHp;
    int redHp, yellowHp;
    int hp;
    int current;
    byte currentWidth, currenColor;
    private void RefreshUI()
    {
      bar.Width = currentWidth;
      if (currenColor == 0)
      {
        bar.Background = RED;
        bar.BorderBrush = REDSHADOW;
      }
      else if (currenColor == 1)
      {
        bar.Background = YELLOW;
        bar.BorderBrush = YELLOWSHADOW;
      }
      else
      {
        bar.Background = GREEN;
        bar.BorderBrush = GREENSHADOW;
      }
      bar.Background = currenColor == 0 ? RED : currenColor == 1 ? YELLOW : GREEN;
    }
    private byte GetWidth()
    {
      int x2 = current * 48 / maxHp;
      return (byte)(x2 == 0 && current != 0 ? 1 : x2);
    }
    private void CurrentChanged()
    {
      byte w, c;
      w = GetWidth();
      c = (byte)(current <= redHp ? 0 : current <= yellowHp ? 1 : 2);
      if (w != currentWidth || c != currenColor)
      {
        currentWidth = w;
        currenColor = c;
        UIDispatcher.Invoke(RefreshUI);
      }
    }
    bool animating;
    private void StartTimer()
    {
      lock (this)
      {
        animating = true;
        timer.Change(0, PERIOD);
      }
    }
    private void StopTimer()
    {
      lock (this)
      {
        animating = false;
        timer.Change(Timeout.Infinite, PERIOD);
      }
    }
    private void TimerCallback()
    {
      if (current == hp)
      {
        StopTimer();
        UIDispatcher.Invoke((Action<Storyboard>)BeginStoryboard, storyboard);
      }
      else
      {
        if (current < hp) ++current;
        else if (current > hp) --current;
        CurrentChanged();
      }
    }

    private void LifeChanged(object sender, PropertyChangedEventArgs e)
    {
      flash.Width = bar.Width;
      hp =((PairValue)sender).Value;
      if (!(animating || current == hp)) StartTimer();
    }
    private void LifeBar_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (e.OldValue is PairValue) ((PairValue)e.OldValue).PropertyChanged -= LifeChanged;
      PairValue hp = DataContext as PairValue;
      if (hp != null)
      {
        hp.PropertyChanged += LifeChanged;
        maxHp = hp.Origin;
        yellowHp = maxHp >> 1;
        redHp = maxHp / 5;
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
