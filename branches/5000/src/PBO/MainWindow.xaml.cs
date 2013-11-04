﻿using System;
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

namespace PokemonBattleOnline.PBO
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    readonly GridLength GL0;
    readonly GridLength GLMIN;

    static MainWindow()
    {
      RoomWindow.Init();
    }

    public MainWindow()
    {
      InitializeComponent();
      Loaded += switchLobby_Click;
      GL0 = new GridLength(0);
      GLMIN = new GridLength(lobby.MinWidth);
      editor.Init();
    }

    private void RefreshGrid()
    {
      double x = Helper.Random.Next(16);
      double y = -Helper.Random.Next(16);
      editor.SetGridBg(-x, y);
      lobby.SetGridBg(-((c0.ActualWidth - 3 + x) % 16), y);
    }

    private void switchLobby_Click(object sender, RoutedEventArgs e)
    {
      if (c1.ActualWidth == 0) c1.Width = GLMIN;
      else c1.Width = new GridLength(grid.ActualWidth);
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
      else if (grid.ActualWidth - w < 321)
      {
        if (grid.ActualWidth - w > 20) c1.Width = new GridLength(grid.ActualWidth - 321);
        else c1.Width = new GridLength(grid.ActualWidth);
      }
      RefreshGrid();
    }
    private void Grid_Loaded(object sender, RoutedEventArgs e)
    {
      c1.Width = GLMIN;
    }
    private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (e.WidthChanged)
      {
        if (c1.ActualWidth == e.PreviousSize.Width) c1.Width = new GridLength(e.NewSize.Width);
        RefreshGrid();
      }
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      e.Cancel = RoomWindow.Window_Closing(this) || lobby.Window_Closing() || editor.Window_Closing();
    }
    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);
      Config.Current.Save();
      Application.Current.Shutdown();
    }
  }
}