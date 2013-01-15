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
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Messaging;

namespace PokemonBattleOnline.PBO.Lobby
{
  /// <summary>
  /// Interaction logic for GlanceLobbies.xaml
  /// </summary>
  public partial class Login : UserControl
  {
    const int PORT = 9898;

    public event Action LoginComplete = delegate { };
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
          //PBOClient.Prepare4Login(ip, PORT);
          //PBOClient.Client.LoginFailed += client_LoginFailed;
          //PBOClient.Client.LoginCompleted += client_LoginComplete;
          //PBOClient.Client.Login(name.Text.Trim(), avatarVM.InnerAvatarId, avatarUrl.Text);//"http://tb.himg.baidu.com/sys/portrait/item/f543c7aec9f1b2bbcac76c6f6c69bfd85603"
        }
        IsEnabled = false;
      }
    }
    private void avatarUrl_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (timer.IsEnabled) timer.Stop();
      timer.Start();
    }

    private void servers_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            button_Click(sender, e);
    }
  }
}
