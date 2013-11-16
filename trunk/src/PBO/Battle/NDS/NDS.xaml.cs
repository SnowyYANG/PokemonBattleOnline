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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.PBO.Battle
{
  /// <summary>
  /// Interaction logic for Simulation.xaml
  /// </summary>
  public partial class NDS : UserControl
  {
    public event Action<SimPokemon> ReviewPokemon
    {
      add { cp.ReviewPokemon += value; }
      remove { cp.ReviewPokemon -= value; }
    }
    
    public NDS()
    {
      InitializeComponent();
      subtitle.VisibilityChanged += (v) => opms.Visibility = v == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }

    internal void Init(RoomController userController)
    {
      var game = userController.Game;
      var controlPanel = new ControlPanelVM(userController);
      cp.Init(controlPanel);
      subtitle.Init(controlPanel);
      int observerTeamId;
      if (userController.PlayerController != null) observerTeamId = userController.PlayerController.Player.Team;
      else observerTeamId = 0;
      opms.ItemsSource = game.Board.Pokemons[observerTeamId];
      rpms.ItemsSource = game.Board.Pokemons[1 - observerTeamId];
      board.Init(game.Board, observerTeamId);
    }
  }
}
