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
using System.Collections.ObjectModel;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.PBO.Battle.VM;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for Simulation.xaml
  /// </summary>
  public partial class NDS : UserControl
  {
    public NDS()
    {
      InitializeComponent();
    }

    internal void Init(Messaging.Room.IRoom userController)
    {
      var game = userController.Game;
      int observerTeamId;
      if (userController.PlayerController != null) observerTeamId = userController.PlayerController.Player.TeamId;
      else observerTeamId = 0;
      opms.ItemsSource = game.Board.Teams[observerTeamId];
      rpms.ItemsSource = game.Board.Teams[1 - observerTeamId];
      IControlPanel controlPanel;
      switch (game.Settings.Mode)
      {
        case GameMode.Single:
          controlPanel = new Singles(userController);
          break;
        default:
          System.Diagnostics.Debugger.Break();
          controlPanel = null;
          break;
      }
      cp.Init(controlPanel);
      subtitle.Init(controlPanel);
      board.Init(game.Board, observerTeamId);

      controlPanel.PropertyChanged += (sender, e) =>
        {
          const string CP = "ControllingPokemon";
          if (e.PropertyName == null || e.PropertyName == CP)
          {
            var p = (sender as IControlPanel).ControllingPokemon;
            //if (p == null) opms.SelectedIndex = -1;
            //else opms.SelectedIndex = p.X;
          }
        };
    }
  }
}
