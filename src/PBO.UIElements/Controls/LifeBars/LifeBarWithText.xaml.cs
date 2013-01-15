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
  /// Interaction logic for LifeBarWithText.xaml
  /// </summary>
  public partial class LifeBarWithText : Canvas
  {
    private const int PERIOD = 16;

    private static void TimerCallback(object o)
    {
      ((LifeBarWithText)o).TimerCallback();
    }

    private readonly Storyboard storyboard;
    private readonly Timer timer;
    private readonly Delegate RefreshAllDelegate;
    private readonly Delegate RefreshTextWidthDelegate;
    private readonly Delegate RefreshTextColorDelegate;
    private readonly Delegate RefreshTextDelegate;

    public LifeBarWithText()
    {
      InitializeComponent();
      storyboard = (Storyboard)Resources["Flash"];
      timer = new Timer(TimerCallback, this, Timeout.Infinite, PERIOD);
      RefreshAllDelegate = new Action(RefreshAll);
      RefreshTextWidthDelegate = new Action(RefreshTextWidth);
      RefreshTextColorDelegate = new Action(RefreshTextColor);
      RefreshTextDelegate = new Action(RefreshText);
    }

    int maxHp;
    int redHp, yellowHp;
    int hp;
    int current;
    byte currentColor;
    double currentWidth;
    private void RefreshText()
    {
      Current.Text = current.ToString();
    }
    private void _RefreshColor()
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
    private void _RefreshWidth()
    {
      bar.Width = currentWidth;
    }
    private void RefreshTextWidth()
    {
      RefreshText();
      _RefreshWidth();
    }
    private void RefreshTextColor()
    {
      RefreshText();
      _RefreshColor();
    }
    private void RefreshAll()
    {
      RefreshText();
      _RefreshColor();
      _RefreshWidth();
    }
    private void CurrentChanged()
    {
      byte c;
      double w = LifeBarHelper.GetWidth(current, maxHp);
      c = (byte)(current <= redHp ? 0 : current <= yellowHp ? 1 : 2);
      Delegate d;
      if (w != currentWidth)
      {
        currentWidth = w;
        if (c != currentColor)
        {
          currentColor = c;
          d = RefreshAllDelegate;
        }
        else d = RefreshTextWidthDelegate;
      }
      else if (c != currentColor)
      {
        currentColor = c;
        d = RefreshTextColorDelegate;
      }
      else d = RefreshTextDelegate;
      UIDispatcher.BeginInvoke(d);
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
        Max.Text = maxHp.ToString();
        Current.Text = current.ToString();
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
