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
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.PBO.Lobby
{
  /// <summary>
  /// Interaction logic for GlanceLobbies.xaml
  /// </summary>
  public partial class Login : UserControl
  {
    public event Action LoginComplete = delegate { };

    public Login()
    {
      InitializeComponent();
      PBOClient.LoginFailed_Name += () => System.Diagnostics.Debugger.Break();
      PBOClient.LoginFailed_Version += () => System.Diagnostics.Debugger.Break();
      PBOClient.LoginFailed_Full += () => System.Diagnostics.Debugger.Break();
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
      if (!string.IsNullOrWhiteSpace(servers.Text) && !string.IsNullOrWhiteSpace(name.Text))
      {
        PBOClient.Login(servers.Text.Trim(), name.Text.Trim(), 1);
        IsEnabled = false;
      }
    }

    private void servers_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter) button_Click(button, e);
    }
  }
}
