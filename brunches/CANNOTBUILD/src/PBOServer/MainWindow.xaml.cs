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
    Dictionary<int, UserVM> usersDictionary;
    ObservableCollection<UserVM> users;

    public MainWindow()
    {
      InitializeComponent();
      TaskbarIconService.Init(this);
      StartServer();
    }

    private void AddUser(User u)
    {
      UserVM uvm= new UserVM(u, true);
      usersDictionary.Add(u.Id, uvm);
      users.Add(uvm);
      chat.AppendText("\n<SYSTEM> " + u.Name + " logs in, ID. " + u.Id);
    }
    private void StartServer()
    {
      try
      {
        PBOServer.NewTcpServer(PBOMarks.DEFAULT_PORT);
        server = PBOServer.Current;
        //server.UserChanged += model_UserChanged;
        //server.MessageBroadcast += model_MessageBroadcast;
        server.Start();
      }
      catch (Exception e)
      {
        System.Windows.MessageBox.Show(e.Message);
        return;
      }
      mask.Visibility = System.Windows.Visibility.Hidden;
      usersDictionary = new Dictionary<int, UserVM>(50);//容量
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

    void model_UserChanged(int userId)
    {
      ////thread
      //WpfDispatcher.Invoke(() =>
      //  {
      //    User u = server.GetUser(userId);
      //    UserVM uvm = usersDictionary.ValueOrDefault(userId);
      //    if (u == null)
      //    {
      //      usersDictionary.Remove(userId);
      //      users.Remove(uvm);
      //      string x = "\n<SYSTEM> User"+ userId;
      //      if (uvm != null) x += " " + uvm.Name;
      //      chat.AppendText(x + " exits.");
      //    }
      //    else
      //    {
      //      if (uvm == null) AddUser(u);
      //      else uvm.RefreshProperties();
      //    }
      //  });
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
      e.Cancel = true;
      Visibility = System.Windows.Visibility.Collapsed;
      base.OnClosing(e);
    }
    protected override void OnClosed(EventArgs e)
    {
      try { StopServer(); }
      catch { }
      finally { server.Dispose(); }
      base.OnClosed(e);
    }
  }
}
