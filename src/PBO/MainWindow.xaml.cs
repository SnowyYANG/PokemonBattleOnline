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
using System.Windows.Shapes;
using System.Net;
using LightStudio.Tactic.Globalization;
using LightStudio.PokemonBattle.PBO.Lobby;
using LightStudio.PokemonBattle.PBO.Battle;
using LightStudio.PokemonBattle.PBO.Editor;

namespace LightStudio.PokemonBattle.PBO
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    readonly GridLength GL0;
    readonly GridLength GLMIN;
    double border;
    
    public MainWindow()
    {
      InitializeComponent();
      Loaded += switchLobby_Click;
      Messaging.BattleClient.EnterSucceed += (user) =>
        new BattleWindow(user).Show();
      GL0 = new GridLength(0);
      GLMIN = new GridLength(lobby.MinWidth);
      editor.Init();
    }

    private void switchLobby_Click(object sender, RoutedEventArgs e)
    {
      if (c1.ActualWidth == 0) c1.Width = GLMIN;
      else c1.Width = new GridLength(ActualWidth - border);
    }
    private void switchEditor_Click(object sender, RoutedEventArgs e)
    {
      if (c0.ActualWidth == 0) c1.Width = GLMIN;
      else c1.Width = GL0;
    }
    private void Rectangle_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      double w = e.NewSize.Width;
      if (w < lobby.MinWidth && w > 0)
      {
        if (e.PreviousSize.Width == lobby.MinWidth) c1.Width = GL0;
        else c1.Width = GLMIN;
      }
      else if (ActualWidth - border - w < 321)
      {
        if (ActualWidth - border - w > 20) c1.Width = new GridLength(ActualWidth - border - 321);
        else c1.Width = new GridLength(ActualWidth - border);
      }
    }
    private void Grid_Loaded(object sender, RoutedEventArgs e)
    {
      border = ActualWidth - ((Grid)sender).ActualWidth;
    }
    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (e.WidthChanged)
        if (c1.ActualWidth == e.PreviousSize.Width - border) c1.Width = new GridLength(ActualWidth - border);
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      e.Cancel = BattleWindow.Window_Closing(this) || lobby.Window_Closing() || editor.Window_Closing();
    }
  }
}
