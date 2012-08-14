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
using LightStudio.PokemonBattle.Messaging;
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
        br.AddLogText(string.Format(DataService.String["Game end {0} : {1}"], team0, team1)));
    }
    void IRoomEventsListener.GameTie()
    {
      UIDispatcher.Invoke(() => br.AddLogText(DataService.String["Game tied"]));
    }
    void IRoomEventsListener.GameStop(GameStopReason reason, int player)
    {
      UIDispatcher.Invoke(() =>
        {
          string formatKey;
          switch (reason)
          {
            case GameStopReason.InvalidInput:
              formatKey = "{0} commands something wrong to pokemon(s). Game stopped.\n";
              break;
            case GameStopReason.RoomClosed:
              formatKey = "The room is closed by room administrator. Game stopped.\n";
              break;
            case GameStopReason.ServerClosed: //这个不是直接连接与服务器中断么
              formatKey = "Disconnected from server.\n";
              break;
            case GameStopReason.PlayerDisconnect:
              formatKey = "Player {0} disconnected. Game stopped.\n";
              break;
            case GameStopReason.PlayerQuit:
              formatKey = "Player {0} quitted. Game stopped.\n";
              break;
            default:
              formatKey = "Disconnected from room.\n";
              break;
          }
          string playerName = player == 0 ? null : PBOClient.GetName(player);
          br.AddLogText(string.Format(DataService.String[formatKey], playerName));
        });
    }
    void IRoomEventsListener.TimeReminder(int[] waitForWhom)
    {
      UIDispatcher.Invoke(() =>
        {
          var names = (from p in waitForWhom select PBOClient.GetName(p)).ToArray();
          switch (waitForWhom.Length)
          {
            case 1: //双打三打pm复数
              br.AddLogText(string.Format(DataService.String["Waiting for {0}'s command to pokemon.\n"], names[0]));
              break;
            case 2:
              br.AddLogText(string.Format(DataService.String["Waiting for {0} and {1}'s commands to pokemons.\n"], names[0], names[1]));
              break;
            case 3:
              br.AddLogText(string.Format(DataService.String["Waiting for {0}, {1} and {2}'s commands to pokemons.\n"], names[0], names[1], names[2]));
              break;
          }
        });
    }
    void IRoomEventsListener.TimeUp(IEnumerable<KeyValuePair<int, int>> spentTime)
    {
      UIDispatcher.Invoke(() =>
        br.AddLogText(DataService.String["TimeUp"]));
    }
    void IRoomEventsListener.Error(string message)
    {
      UIDispatcher.Invoke(() =>
        br.AddLogText("<error>" + message));
    }
    #endregion
  }
}
