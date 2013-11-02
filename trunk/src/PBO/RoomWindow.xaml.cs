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
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.PBO
{
  /// <summary>
  /// Interaction logic for BattleWindow.xaml
  /// </summary>
  public partial class RoomWindow : Window
  {
    #region static
    private static RoomWindow Current;
    
    public static void Init()
    {
      RoomController.Quited += RoomController_Quited;
      RoomController.GameStop += RoomController_GameStop;
      RoomController.RoomChat += RoomController_RoomChat;
      RoomController.TimeReminder += RoomController_TimeReminder;
      RoomController.TimeUp += RoomController_TimeUp;
      RoomController.Entered += RoomController_Entered;
      Current = new RoomWindow() { Visibility = Visibility.Collapsed };
    }

    static void RoomController_Entered()
    {
      Current.Show();
    }

    static void RoomController_TimeUp(IEnumerable<KeyValuePair<User, int>> spentTime)
    {
      var br = Current.br;
      br.AddLogText("Time Up\r\n");
      foreach (var pair in spentTime)
        br.AddLogText(string.Format("{0}使用了{1}秒\r\n", pair.Key, pair.Value));
    }
    static void RoomController_TimeReminder(User[] users)
    {
      var br = Current.br;
      switch (users.Length)
      {
        case 1: //双打三打pm复数
          br.AddLogText(string.Format("等待{0}给精灵下达命令。\r\n", users[0].Name));
          break;
        case 2:
          br.AddLogText(string.Format("等待{0}和{1}给精灵下达命令。\r\n", users[0].Name, users[1].Name));
          break;
        case 3:
          br.AddLogText(string.Format("等待{0}、{1}和{2}给精灵下达命令。\r\n", users[0].Name, users[1].Name, users[2].Name));
          break;
      }
    }
    static void RoomController_RoomChat(string arg1, User arg2)
    {
      var br = Current.br;
      br.AddChatText(arg1, arg2);
    }
    static void RoomController_GameStop(GameStopReason reason, User player)
    {
      string formatKey;
      switch (reason)
      {
        case GameStopReason.InvalidInput:
          formatKey = "{0}给精灵下达了错误的命令。游戏中止。";
          break;
        case GameStopReason.PlayerGiveUp:
          formatKey = "{0}选择了投降。";
          break;
        default://case GameStopReason.PlayerDisconnect:
          formatKey = "{0}断线了。游戏中止。";
          break;
      }
      Current.br.AddLogText(string.Format(formatKey.LineBreak(), player.Name));
    }
    static void RoomController_Quited()
    {
      Current.Visibility = Visibility.Collapsed;
    }

    public static bool Window_Closing(Window mainWindow)
    {
      if (PBOClient.Current != null && PBOClient.Current.Controller.User.Room != null)
      {
        ShowMessageBox.CantCloseMainWindow(mainWindow);
        return true;
      }
      return false;
    }
    #endregion

    private RoomController Room;

    public RoomWindow()
    {
      Current = this;
      InitializeComponent();
      nds.ReviewPokemon += (p) => pmReview.Content = p;
      Teams.ItemsSource = Editor.EditorVM.Current.BattleTeams;
      Chat.Speak += Chat_Speak;
    }

    private void Chat_Speak(string chat)
    {
      Room.Chat(chat);
    }

    public new void Show()
    {
      Room = PBOClient.Current.Controller.Room;
      Prepare.DataContext = Room;
      Visibility = System.Windows.Visibility.Visible;
      PX1.Height = new GridLength(Room.Room.Settings.Mode == GameMode.Tag ? 32 : 0);
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      e.Cancel = true;
      if (Room == null || !Room.Room.Battling || ShowMessageBox.ClosingInBattle(this) == MessageBoxResult.Yes)
      {
        br.Reset();
        Prepare.DataContext = null;
        Room.Quit();
      }
    }

    private void OnGameStart()
    {
      StringBuilder t0 = new StringBuilder();
      StringBuilder t1 = new StringBuilder();
      foreach (var p in Room.Room.Players)
        if (p.Seat.TeamId() == 0)
        {
          t0.Append(p.Name);
          t0.Append(' ');
        }
        else
        {
          t1.Append(' ');
          t1.Append(p.Name);
        }
      t0.Append("VS");
      t0.Append(t1);
      var title = t0.ToString();
      Title = title;

      nds.Init(Room);
      br.Init(Room.Game, title, Room.PlayerController == null ? null : Room.Client.User.Name);
      Room.Game.GameStart += () => Prepare.Visibility = Visibility.Collapsed;
    }
  }
}
