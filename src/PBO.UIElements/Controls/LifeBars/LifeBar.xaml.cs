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

namespace PokemonBattleOnline.PBO.UIElements
{
  /// <summary>
  /// Interaction logic for LifeBar.xaml
  /// </summary>
  public partial class LifeBar : Canvas
  {
    private const int PERIOD = 16;
    private static void TimerCallback(object o)
    {
      ((LifeBar)o).TimerCallback();
    }

    private readonly Storyboard storyboard;
    private readonly Timer timer;
    private readonly Delegate RefreshWidthDelegate;
    private readonly Delegate RefreshColorDelegate;
    private readonly Delegate RefreshBothDelegate;
    
    public LifeBar()
    {
      InitializeComponent();
      storyboard = (Storyboard)Resources["Flash"];
      timer = new Timer(TimerCallback, this, Timeout.Infinite, PERIOD);
      RefreshWidthDelegate = new Action(RefreshWidth);
      RefreshColorDelegate = new Action(RefreshColor);
      RefreshBothDelegate = new Action(RefreshBoth);
    }

    int maxHp;
    int redHp, yellowHp;
    int hp;
    int current;
    byte currentColor;
    double currentWidth;
    private void RefreshWidth()
    {
      bar.Width = currentWidth;
    }
    private void RefreshColor()
    {
      if (currentColor == 0)
      {
        bar.Background = LifeBarHelper.RED;
        bar.BorderBrush = LifeBarHelper.REDSHADOW;
      }
      else if (currentColor == 1)
      {
        bar.Background = LifeBarHelper.YELLOW;
        bar.BorderBrush = LifeBarHelper.YELLOWSHADOW;
      }
      else
      {
        bar.Background = LifeBarHelper.GREEN;
        bar.BorderBrush = LifeBarHelper.GREENSHADOW;
      }
    }
    private void RefreshBoth()
    {
      RefreshWidth();
      RefreshColor();
    }
    private void CurrentChanged()
    {
      byte c;
      double w;
      w = LifeBarHelper.GetWidth(current, maxHp);
      c = (byte)(current <= redHp ? 0 : current <= yellowHp ? 1 : 2);
      if (w != currentWidth)
      {
        currentWidth = w;
        if (c != currentColor)
        {
          currentColor = c;
          UIDispatcher.BeginInvoke(RefreshBothDelegate);
        }
        else UIDispatcher.BeginInvoke(RefreshWidthDelegate);
      }
      else if (c != currentColor)
      {
        currentColor = c;
        UIDispatcher.BeginInvoke(RefreshColorDelegate);
      }
    }
    #region timer
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
    #endregion

    private void LifeChanged(object sender, PropertyChangedEventArgs e)
    {
      flash.Width = bar.Width;
      hp =((PairValue)sender).Value;
      if (!(animating || current == hp)) StartTimer();
    }
    private void LifeBar_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (e.OldValue is PairValue) ((PairValue)e.OldValue).PropertyChanged -= LifeChanged;
      PairValue pair = DataContext as PairValue;
      if (pair != null)
      {
        pair.PropertyChanged += LifeChanged;
        maxHp = pair.Origin;
        yellowHp = maxHp >> 1;
        redHp = maxHp / 5;
        current = hp = pair.Value;
        CurrentChanged();
        flash.Width = LifeBarHelper.GetWidth(hp, maxHp);
      }
    }
    private void Storyboard_Completed(object sender, EventArgs e)
    {
      flash.Width = LifeBarHelper.GetWidth(hp, maxHp);
    }
  }
}
