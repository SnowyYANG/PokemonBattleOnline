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
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.PBO.Lobby
{
  /// <summary>
  /// Interaction logic for GlanceLobbies.xaml
  /// </summary>
  public partial class Login : UserControl
  {
    public Login()
    {
      InitializeComponent();
      PBOClient.LoginFailed_Name += () => LoginFailed(R.LOGINFAILED_NAME);
      PBOClient.LoginFailed_Version += () => LoginFailed(R.LOGINFAILED_VERSION);
      PBOClient.LoginFailed_Full += () => LoginFailed(R.LOGINFAILED_FULL);
      PBOClient.CurrentChanged += () => IsEnabled = true;
      avatar.Content = 821;
    }

    private void LoginFailed(string message)
    {
      MessageBox.Show(message);
      IsEnabled = true;
    }
    private void button_Click(object sender, RoutedEventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(servers.Text) && !string.IsNullOrWhiteSpace(name.Text))
      {
        PBOClient.Login(servers.Text.Trim(), name.Text.Trim(), 1);
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
