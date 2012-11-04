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
using LightStudio.PokemonBattle.Messaging;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Lobby
{
  /// <summary>
  /// Interaction logic for LobbyWindow.xaml
  /// </summary>
  public partial class LobbyPanel : UserControl
  {
    public LobbyPanel()
    {
      InitializeComponent();
      login.LoginComplete += () =>
        {
          PBOClient.Client.Disconnected += (sender, e) => UIDispatcher.Invoke(() =>
            {
              lobby.Init(null);
              login.Visibility = Visibility.Visible;
            });
          login.Visibility = Visibility.Hidden;
          lobby.Init(new LobbyVM());
        };
    }

    public bool Window_Closing()
    {
      return lobby.Window_Closing();
    }
  }
}
