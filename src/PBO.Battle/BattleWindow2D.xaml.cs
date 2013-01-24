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
using System.Collections.ObjectModel;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;
using PokemonBattleOnline.Network.Room;
using PokemonBattleOnline.PBO.Battle.VM;

namespace PokemonBattleOnline.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BattleWindow.xaml
  /// </summary>
  public partial class BattleWindow : Window, IRoomEventsListener
  {
    private static readonly Collection<BattleWindow> Instances = new Collection<BattleWindow>();
    public static bool Window_Closing(Window mainWindow)
    {
      if (Instances.FirstOrDefault() != null)
      {
        UIElements.ShowMessageBox.CantCloseMainWindow(mainWindow);
        return true;
      }
      return false;
    }
    
    private readonly IRoom Room;

    public BattleWindow(IRoom room)
    {
      InitializeComponent();
      Room = room;
      room.AddListener(this);
      Instances.Add(this);
      nds.ReviewPokemon += (p) => pmReview.Content = p;
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      if (Room != null && Room.RoomState != RoomState.GameEnd)
      {
        var result = UIElements.ShowMessageBox.ClosingInBattle(this);
        e.Cancel = result != MessageBoxResult.Yes;
      }
    }

    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);
      Room.Quit();
      Instances.Remove(this);
    }
    
    #region IRoomEventsListener
    void IRoomEventsListener.GameStart()
    {
      throw new NotImplementedException();
      //UIDispatcher.Invoke(() =>
      //  {
      //    StringBuilder t0 = new StringBuilder();
      //    StringBuilder t1 = new StringBuilder();
      //    foreach (var p in Room.Players)
      //      if (p.Team == 0)
      //      {
      //        t0.Append(p.Name);
      //        t0.Append(' ');
      //      }
      //      else
      //      {
      //        t1.Append(' ');
      //        t1.Append(p.Name);
      //      }
      //    t0.Append("VS");
      //    t0.Append(t1);
      //    var title = t0.ToString();
      //    Title = title;

      //    nds.Init(Room);
      //    br.Init(Room.Game, title, Room.PlayerController == null ? null : PBOClient.GetName(Room.PlayerController.Player.Id));
      //    Room.Game.LeapTurn += () => mask.Visibility = Visibility.Collapsed;
      //  });
    }
    void IRoomEventsListener.GameTie()
    {
      UIDispatcher.Invoke(() => br.AddLogText(DataService.String["Game tied"]));
    }
    void IRoomEventsListener.GameStop(GameStopReason reason, int player)
    {
      throw new NotImplementedException();
      //UIDispatcher.Invoke(() =>
      //  {
      //    string formatKey;
      //    switch (reason)
      //    {
      //      case GameStopReason.InvalidInput:
      //        formatKey = "{0} commands something wrong to pokemon(s). Game stopped.";
      //        break;
      //      case GameStopReason.RoomClosed:
      //        formatKey = "The room is closed by room administrator. Game stopped.";
      //        break;
      //      case GameStopReason.ServerClosed: //这个不是直接连接与服务器中断么
      //        formatKey = "Disconnected from server.";
      //        break;
      //      case GameStopReason.PlayerDisconnect:
      //        formatKey = "Player {0} disconnected. Game stopped.";
      //        break;
      //      case GameStopReason.PlayerGiveUp:
      //        formatKey = "Player {0} chooses to surrender.";
      //        break;
      //      default:
      //        formatKey = "Disconnected from room.";
      //        break;
      //    }
      //    string playerName = player == 0 ? null : PBOClient.GetName(player);
      //    br.AddLogText(string.Format(DataService.String[formatKey] + "\n", playerName));
      //  });
    }
    void IRoomEventsListener.TimeReminder(int[] waitForWhom)
    {
      throw new NotImplementedException();
      //UIDispatcher.Invoke(() =>
      //  {
      //    var names = (from p in waitForWhom select PBOClient.GetName(p)).ToArray();
      //    switch (waitForWhom.Length)
      //    {
      //      case 1: //双打三打pm复数
      //        br.AddLogText(string.Format(DataService.String["Waiting for {0}'s command to pokemon."], names[0]) + "\n");
      //        break;
      //      case 2:
      //        br.AddLogText(string.Format(DataService.String["Waiting for {0} and {1}'s commands to pokemons."], names[0], names[1]) + "\n");
      //        break;
      //      case 3:
      //        br.AddLogText(string.Format(DataService.String["Waiting for {0}, {1} and {2}'s commands to pokemons."], names[0], names[1], names[2]) + "\n");
      //        break;
      //    }
      //  });
    }
    void IRoomEventsListener.TimeUp(IEnumerable<KeyValuePair<int, int>> spentTime)
    {
      UIDispatcher.Invoke(() =>
        {
          br.AddLogText(DataService.String["Time Up"] + "\n");
          foreach(var pair in spentTime)
            br.AddLogText(string.Format(Room.Game, "{0:P}使用了{1}秒", pair.Key, pair.Value) + "\n");
        });
    }
    void IRoomEventsListener.Error(string message)
    {
      UIDispatcher.Invoke(() =>
        br.AddLogText("<error>" + message));
    }
    #endregion
  }
}
