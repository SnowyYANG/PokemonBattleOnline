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
using LightStudio.PokemonBattle.Room;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.PBO.Battle.VM;
using LightStudio.Tactic.Logging;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BattleWindow.xaml
  /// </summary>
  public partial class BattleWindow : Window
  {
    IUserController userController;

    public BattleWindow(IUserController controller)
    {
      InitializeComponent();
      Init(controller);
    }

    internal void Init(IUserController controller)
    {
      if (DataContext != null) return;
      userController = controller;
      nds.Init(controller);
      br.Init(controller.Game);
      controller.Game.LeapTurn += () =>
        {
          mask.Visibility = System.Windows.Visibility.Collapsed;
        };
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      if (userController != null && userController.RoomState != RoomState.GameEnd)
      {
        var result = UIElements.ShowMessageBox.ClosingInBattle(this);
        e.Cancel = result != MessageBoxResult.Yes;
      }
    }

    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);
      userController.Quit();
    }
  }
}
