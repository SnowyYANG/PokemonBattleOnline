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

    public event Action<PokemonLobbyClient> LoginComplete = delegate { };
    private PokemonLobbyClient currentClient;
    private DispatcherTimer timer;
    private AvatarVM avatarVM;

    public Login()
    {
      avatarVM = new AvatarVM(0, null);
      timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1.5) };
      timer.Tick += (sender, e) =>
        {
          avatarVM.SetUrl(avatarUrl.Text);
          timer.Stop();
        };
      InitializeComponent();
      avatar.Content = avatarVM;
    }

    private void client_LoginComplete() //not in UI thread
    {
      lock (this)
      {
        UIDispatcher.Invoke(() =>
          {
            currentClient.LoginCompleted -= client_LoginComplete;
            LoginComplete(currentClient);
            currentClient = null;
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
            currentClient = null;
            IsEnabled = true;
          });
      }
    }
    private void button_Click(object sender, RoutedEventArgs e)
    {
      //Is it neccessary to make UserData multi-instances?
      string addr = servers.Text.Trim();
      if (currentClient != null || string.IsNullOrWhiteSpace(addr) || string.IsNullOrWhiteSpace(name.Text)) return;
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
          currentClient = new PokemonLobbyClient(ip, PORT);
          currentClient.LoginFailed += client_LoginFailed;
          currentClient.LoginCompleted += client_LoginComplete;
          currentClient.Login(name.Text.Trim(), avatarVM.InnerAvatarId, avatarUrl.Text);//"http://tb.himg.baidu.com/sys/portrait/item/f543c7aec9f1b2bbcac76c6f6c69bfd85603"
        }
        IsEnabled = false;
      }
    }
    private void avatarUrl_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (timer.IsEnabled) timer.Stop();
      timer.Start();
    }



    private void textAvatarUrlHidden(object sender, RoutedEventArgs e)
    {
        this.textAvatarUrl.Visibility = Visibility.Hidden;
    }

    private void textAvatarUrlVisible(object sender, RoutedEventArgs e)
    {
        if(this.avatarUrl.Text.Length == 0)
            this.textAvatarUrl.Visibility = Visibility.Visible;
    }

    private void textNameVisible(object sender, RoutedEventArgs e)
    {
        if (this.name.Text.Length == 0)
            this.textName.Visibility = Visibility.Visible;
    }

    private void textNameHidden(object sender, RoutedEventArgs e)
    {
        this.textName.Visibility = Visibility.Hidden;
    }  
  }
}
