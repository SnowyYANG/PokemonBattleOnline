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
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Messaging.Room;
using LightStudio.PokemonBattle.PBO.Battle.VM;
using LightStudio.Tactic.Logging;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BattleWindow.xaml
  /// </summary>
  public partial class BattleWindow : Window
  {
    IRoom room;

    public BattleWindow(IRoom controller)
    {
      InitializeComponent();
      controller.GameStart += () =>
        {
          room = controller;
          UIDispatcher.Invoke(Init);
        };
    }

    internal void Init()
    {
      if (DataContext != null) return;
      nds.Init(room);
      br.Init(room.Game);
      room.Game.LeapTurn += () =>
        {
          mask.Visibility = System.Windows.Visibility.Collapsed;
        };
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      if (room != null && room.RoomState != RoomState.GameEnd)
      {
        var result = UIElements.ShowMessageBox.ClosingInBattle(this);
        e.Cancel = result != MessageBoxResult.Yes;
      }
    }

    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);
      room.Quit();
    }
  }
}
