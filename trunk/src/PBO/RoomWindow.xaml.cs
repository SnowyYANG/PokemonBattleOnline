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
      RoomController.GameStop += (r, p) => Current.OnGameStop(r, p);
      RoomController.RoomChat += RoomController_RoomChat;
      RoomController.TimeReminder += RoomController_TimeReminder;
      RoomController.TimeUp += RoomController_TimeUp;
      RoomController.Entered += RoomController_Entered;
      RoomController.GameInited += RoomController_GameInited;
      PBOClient.CurrentChanged += PBOClient_CurrentChanged;
      Current = new RoomWindow() { Visibility = Visibility.Collapsed };
    }

    static void PBOClient_CurrentChanged()
    {
      if (PBOClient.Current == null) Current.Reset(null);
    }

    static void RoomController_GameInited()
    {
      Current.OnGameInited();
    }

    static void RoomController_Entered()
    {
      Current.Reset(PBOClient.Current.Room);
    }

    static void RoomController_TimeUp(IEnumerable<KeyValuePair<User, int>> spentTime)
    {
      var br = Current.br;
      br.AddLogText("Time Up\r\n");
      foreach (var pair in spentTime)
        br.AddUserText(string.Format("{0}使用了{1}秒", pair.Key.Name, pair.Value), pair.Key);
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
    static void RoomController_Quited()
    {
      Current.Reset(null);
    }

    public static bool Window_Closing(Window mainWindow)
    {
      if (PBOClient.Current != null && PBOClient.Current.User.Room != null)
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
      //nds.ReviewPokemon += (p) => pmReview.Content = p;
      Teams.ItemsSource = Editor.EditorVM.Current.BattleTeams;
      Chat.Speak += Chat_Speak;
    }

    private void Chat_Speak(string chat)
    {
      Room.Chat(chat);
    }
    private void Start_Click(object sender, RoutedEventArgs e)
    {
      var team = Teams.SelectedItem as Editor.TeamVM;
      if (team != null && team.Model.Pokemons[0] != null)
      {
        var pt = team.Model.Pokemons.Where((p) => p != null).ToArray();
        Room.GamePrepare(pt);
        PrepareTeam.DataContext = pt;
        Teams.Visibility = System.Windows.Visibility.Collapsed;
        Start.Content = "等待其他玩家开始对战...";
        Start.IsEnabled = false;
      }
    }

    private void Reset(RoomController room)
    {
      if (room != null)
      {
        Prepare.DataContext = Current.Room = room;
        PX1.Height = new GridLength(room.Room.Settings.Mode == GameMode.Tag ? 32 : 0);
        Visibility = Visibility.Visible;
        Teams.Visibility = Visibility.Visible;
        Prepare.Visibility = Visibility.Visible;
        Start.IsEnabled = true;
        Start.Content = "使用所选队伍开始对战！";
        Title = "对战房间";
      }
      else
      {
        Current.Visibility = Visibility.Collapsed;
        br.Reset();
        Prepare.DataContext = null;
        Room = null;
      }
    }

    private void OnGameInited()
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
      br.Reset();
      br.Init(Room.Game);
      Room.Game.GameStart += () =>
        {
          br.FontSize = 14;
          Prepare.Visibility = Visibility.Collapsed;
        };
    }

    private void OnGameStop(GameStopReason reason, User player)
    {
      if (reason != GameStopReason.GameEnd)
      {
        br.FontSize = 12;
        if (reason == GameStopReason.Error) br.AddLogText("游戏对战逻辑发生了错误，请将战报与队伍发送给反馈人员，谢谢。");
        else br.AddLogText(string.Format(GameString.Current.BattleLog("SYS_" + reason.ToString()).LineBreak(), player.Name));
      }
      if (Room.User.Seat != Seat.Spectator) br.Save(Title, Room.Client.User.Name);
      PrepareTeam.DataContext = null;
      Teams.Visibility = Visibility.Visible;
      Prepare.Visibility = Visibility.Visible;
      Start.IsEnabled = true;
      Start.Content = "使用所选队伍开始对战！";
      Title = "对战房间";
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      e.Cancel = true;
      if (Room != null && (!Room.Room.Battling || ShowMessageBox.ClosingInBattle(this) == MessageBoxResult.Yes)) Room.Quit();
    }
  }
}
