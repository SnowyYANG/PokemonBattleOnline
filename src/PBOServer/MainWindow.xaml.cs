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
using LightStudio.Tactic.Messaging;
using LightStudio.Tactic.Messaging.Lobby;

namespace LightStudio.PokemonBattle.PBO.Server
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    LobbyServer model;
    Dictionary<int, UserVM> usersDictionary;
    ObservableCollection<UserVM> users;

    public MainWindow()
    {
      WpfDispatcher.Init(Dispatcher);
      InitializeComponent();
      StartServer();
    }

    private void AddUser(User u)
    {
      UserVM uvm;
      uvm = new UserVM(u, true);
      usersDictionary.Add(u.Id, uvm);
      users.Add(uvm);
      chat.AppendText("\n<SYSTEM> " + u.Name + " logs in, ID. " + u.Id);
    }
    private void StartServer()
    {
      model = new LobbyServer(new TcpMessageServer(9898));
      model.UserChanged += model_UserChanged;
      model.MessageBroadcast += model_MessageBroadcast;
      try
      {
        model.Start();
      }
      catch (Exception e)
      {
        System.Windows.MessageBox.Show(e.Message);
        return;
      }
      s.Content = "Stop";
      mask.Visibility = System.Windows.Visibility.Hidden;
      usersDictionary = new Dictionary<int, UserVM>(50);//容量
      users = new ObservableCollection<UserVM>();
      var us = model.Users;
      foreach (User u in us) AddUser(u);
      usersView.ItemsSource = users;
    }
    private void StopServer()
    {
      try
      {
        model.Stop();
        model.UserChanged -= model_UserChanged;
        model.MessageBroadcast -= model_MessageBroadcast;
        model.Dispose();
      }
      catch (Exception e)
      {
        System.Windows.MessageBox.Show(e.Message);
        return;
      }
      s.Content = "Start";
      mask.Visibility = System.Windows.Visibility.Visible;
    }

    void model_UserChanged(LobbyServer sender, int userId)
    {
      //thread
      WpfDispatcher.Invoke(() =>
        {
          User u = sender.GetUser(userId);
          UserVM uvm = usersDictionary.ValueOrDefault(userId);
          if (u == null)
          {
            usersDictionary.Remove(userId);
            users.Remove(uvm);
            string x = "\n<SYSTEM> User"+ userId;
            if (uvm != null) x += " " + uvm.Name;
            chat.AppendText(x + " exits.");
          }
          else
          {
            if (uvm == null) AddUser(u);
            else uvm.RefreshProperties(u);
          }
        });
    }
    void model_MessageBroadcast(LobbyServer model, int userId, string content)
    {
      User u = model.GetUser(userId);
      WpfDispatcher.Invoke(() =>
        {
          if (u != null) chat.AppendText("\n" + u.Name + ": " + content);
          else chat.AppendText("\n" + "[" + userId + "]" + ": " + content);
        });
    }
    
    private void Button_Click(object sender, RoutedEventArgs e)
    {
      if (model.IsStarted)
      {
        StopServer();
        ((Button)sender).Content = "Start";
        mask.Visibility = Visibility.Visible;
      }
      else
      {
        StartServer();
        ((Button)sender).Content = "Stop";
        mask.Visibility = Visibility.Hidden;
      }
    }
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = MessageBox.Show("Exit?", "PBO Server", MessageBoxButton.YesNo) == MessageBoxResult.No;
      if (!e.Cancel)
      {
        try { StopServer(); }
        catch { }
        finally
        {
          model.Dispose();
        }
      }
    }
  }
}
