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
using System.Windows.Threading;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Messaging;

namespace LightStudio.PokemonBattle.PBO.Lobby
{
  /// <summary>
  /// Interaction logic for GlanceLobbies.xaml
  /// </summary>
  public partial class Login : UserControl
  {
    const int PORT = 9898;

    public event Action LoginComplete = delegate { };

    public Login()
    {
      InitializeComponent();
      avatar.Content = 821;
    }

    private void client_LoginComplete() //not in UI thread
    {
      lock (this)
      {
        UIDispatcher.Invoke(() =>
          {
            LoginComplete();
            button.IsEnabled = true;
          });
      }
    }
    private void client_LoginFailed()
    {
      lock (this)
      {
        UIDispatcher.Invoke(() =>
          {
            MessageBox.Show("Login Failed");
            IsEnabled = true;
          });
      }
    }
    private void button_Click(object sender, RoutedEventArgs e)
    {
      //Is it neccessary to make UserData multi-instances?
      string addr = servers.Text.Trim();
      if (string.IsNullOrWhiteSpace(addr) || string.IsNullOrWhiteSpace(name.Text)) return;
      System.Net.IPAddress ip;
      if (!System.Net.IPAddress.TryParse(addr, out ip))
        try
        {
          var ips = System.Net.Dns.GetHostAddresses(addr);
          foreach(var i in ips)
            if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
              ip = i;
              break;
            }
        }
        catch { }
      if (ip != null)
      {
        lock (this)
        {
          PBOClient.Prepare4Login(ip, PORT);
          PBOClient.Client.LoginFailed += client_LoginFailed;
          PBOClient.Client.LoginCompleted += client_LoginComplete;
          PBOClient.Client.Login(name.Text.Trim(), (int)avs.SelectedItem);//"http://tb.himg.baidu.com/sys/portrait/item/f543c7aec9f1b2bbcac76c6f6c69bfd85603"
        }
        IsEnabled = false;
      }
    }

    private void avs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      avs.Visibility = System.Windows.Visibility.Collapsed;
      login.Visibility = System.Windows.Visibility.Visible;
    }
    private void avatar_MouseDown(object sender, MouseButtonEventArgs e)
    {
      login.Visibility = System.Windows.Visibility.Collapsed;
      avs.Visibility = System.Windows.Visibility.Visible;
    }
  }
}