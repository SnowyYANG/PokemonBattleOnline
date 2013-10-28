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
using System.Windows.Threading;

namespace PokemonBattleOnline.PBO.Battle
{
  /// <summary>
  /// Interaction logic for Subtitle.xaml
  /// </summary>
  public partial class Subtitle : Border
  {
    DispatcherTimer timer;
    int maxLength;
    int nowLength;
    Control control;

    public Subtitle()
    {
      InitializeComponent();
      timer = new DispatcherTimer();
      timer.Interval = TimeSpan.FromMilliseconds(30);
      timer.Tick += timer_Tick;
      control = new Control(this);
    }
    internal string Text { get; private set; }

    internal void Init(IControlPanel controlPanel)
    {
      controlPanel.PropertyChanged += control.ControlPanel_PropertyChanged;
      controlPanel.InputFailed += control.ControlPanel_InputFailed;
    }

    void timer_Tick(object sender, EventArgs e)
    {
      if (nowLength < maxLength)
        textblock.Text = Text.Substring(0, ++nowLength);
      else control.EventFinished();
    }
    void SetText(string text)
    {
      if (Text != text && text != null)
        SetTextForcibly(text);
    }
    /// <summary>
    /// 即使相同也再更刷一次字幕
    /// </summary>
    /// <param name="text"></param>
    void SetTextForcibly(string text)
    {
      timer.Stop();
      Text = text;
      nowLength = 0;
      maxLength = text.Length;
      timer.Start();
    }
  }
}
