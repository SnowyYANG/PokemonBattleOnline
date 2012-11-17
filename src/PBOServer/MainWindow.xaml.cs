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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Messaging;
using User = LightStudio.Tactic.Messaging.User<LightStudio.PokemonBattle.Messaging.RoomInfo>;

namespace LightStudio.PokemonBattle.PBO.Server
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    PBOServer server;
    Dictionary<int, UserVM> usersDictionary;
    ObservableCollection<UserVM> users;

    public MainWindow()
    {
      WpfDispatcher.Init(Dispatcher);
      InitializeComponent();
      StartServer();
      TaskbarIconService.Init(this);
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
      server = PBOServer.NewTcpServer(9898);
      server.UserChanged += model_UserChanged;
      server.MessageBroadcast += model_MessageBroadcast;
      try
      {
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
      var us = server.Users;
      foreach (User u in us) AddUser(u);
      usersView.ItemsSource = users;
    }
    private void StopServer()
    {
      try
      {
        server.Stop();
        server.UserChanged -= model_UserChanged;
        server.MessageBroadcast -= model_MessageBroadcast;
        server.Dispose();
      }
      catch (Exception e)
      {
        System.Windows.MessageBox.Show(e.Message);
        return;
      }
      mask.Visibility = System.Windows.Visibility.Visible;
    }

    void model_UserChanged(int userId)
    {
      //thread
      WpfDispatcher.Invoke(() =>
        {
          User u = server.GetUser(userId);
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
            else uvm.RefreshProperties();
          }
        });
    }
    void model_MessageBroadcast(int userId, string content)
    {
      User u = server.GetUser(userId);
      WpfDispatcher.Invoke(() =>
        {
          if (u != null) chat.AppendText("\n" + u.Name + ": " + content);
          else chat.AppendText("\n" + "[" + userId + "]" + ": " + content);
        });
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
