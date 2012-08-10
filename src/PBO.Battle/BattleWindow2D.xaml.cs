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
using DataService = LightStudio.PokemonBattle.Data.DataService;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BattleWindow.xaml
  /// </summary>
  public partial class BattleWindow : Window, IRoomEventsListener
  {
    IRoom room;

    public BattleWindow(IRoom room)
    {
      InitializeComponent();
      this.room = room;
      room.AddListener(this);
      room.Quited += () => UIDispatcher.Invoke(() => MessageBox.Show("Quited"));
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

    #region IRoomEventsListener
    void IRoomEventsListener.GameStart()
    {
      UIDispatcher.Invoke(() =>
        {
          nds.Init(room);
          br.Init(room.Game);
          room.Game.LeapTurn += () => mask.Visibility = System.Windows.Visibility.Collapsed;
        });
    }
    void IRoomEventsListener.GameResult(int team0, int team1)
    {
      UIDispatcher.Invoke(() =>
        br.AddText(string.Format(DataService.String["Game end {0} : {1}"], team0, team1)));
    }
    void IRoomEventsListener.GameTie()
    {
      UIDispatcher.Invoke(() => br.AddText(DataService.String["Game tied"]));
    }
    void IRoomEventsListener.GameStop(GameStopReason reason, int player)
    {
      UIDispatcher.Invoke(() =>
        br.AddText(string.Format(DataService.String["Game stop due to {0}'s {1}"], player, reason)));
    }
    void IRoomEventsListener.TimeReminder(int[] waitForWhom)
    {
      UIDispatcher.Invoke(() =>
        br.AddText(string.Format(DataService.String["Waiting for {0}'s commands to pokemons"], waitForWhom)));
    }
    void IRoomEventsListener.TimeUp(int[] remainingTime)
    {
      UIDispatcher.Invoke(() =>
        br.AddText(DataService.String["TimeUp"]));
    }
    void IRoomEventsListener.Error(string message)
    {
      UIDispatcher.Invoke(() =>
        br.AddText("<error>" + message));
    }
    #endregion
  }
}
