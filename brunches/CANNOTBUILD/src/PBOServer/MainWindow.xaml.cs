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
using PokemonBattleOnline.Tactic.Network;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.PBO.Server
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    Network.Server server;
    ObservableCollection<UserVM> users;

    public MainWindow()
    {
      InitializeComponent();
      TaskbarIconService.Init(this);
      StartServer();
    }

    private void StartServer()
    {
      try
      {
        PBOServer.NewTcpServer(PBOMarks.DEFAULT_PORT);
        server = PBOServer.Current;
        server.UsersUpdate += model_UserChanged;
        //server.MessageBroadcast += model_MessageBroadcast;
        server.Start();
      }
      catch (Exception e)
      {
        System.Windows.MessageBox.Show(e.Message);
        return;
      }
      mask.Visibility = System.Windows.Visibility.Hidden;
      users = new ObservableCollection<UserVM>();
      usersView.ItemsSource = users;
    }
    private void StopServer()
    {
      //try
      //{
      //  server.UserChanged -= model_UserChanged;
      //  server.MessageBroadcast -= model_MessageBroadcast;
      //  server.Dispose();
      //}
      //catch (Exception e)
      //{
      //  System.Windows.MessageBox.Show(e.Message);
      //  return;
      //}
      //mask.Visibility = System.Windows.Visibility.Visible;
    }

    void model_UserChanged(User user)
    {
      ////thread
      UIDispatcher.Invoke(() =>
        {
          if (user.State == UserState.Quited)
          {
            //users.Remove(user);
            string x = "\n<SYSTEM> User" + user.Id;
            if (user.Name != null) x += " " + user.Name;
            chat.AppendText(x + " exits.");
          }
          else
          {
            UserVM uvm = new UserVM(user, true);
            users.Add(uvm);
            chat.AppendText("\n<SYSTEM> " + user.Name + " logs in, ID. " + user.Id);
          }
        });
    }
    void model_MessageBroadcast(int userId, string content)
    {
      //User u = server.GetUser(userId);
      //WpfDispatcher.Invoke(() =>
      //  {
      //    if (u != null) chat.AppendText("\n" + u.Name + ": " + content);
      //    else chat.AppendText("\n" + "[" + userId + "]" + ": " + content);
      //  });
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      if (!e.Cancel) e.Cancel = MessageBox.Show("真的要退出吗？", "PBO Server", MessageBoxButton.YesNo) != MessageBoxResult.Yes;
    }
    protected override void OnClosed(EventArgs e)
    {
      try
      {
        TaskbarIconService.Close();
        StopServer();
      }
      catch { }
      finally { server.Dispose(); }
      base.OnClosed(e);
    }
  }
}
