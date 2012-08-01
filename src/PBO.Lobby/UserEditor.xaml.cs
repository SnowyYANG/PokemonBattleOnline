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
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Messaging;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Lobby
{
  /// <summary>
  /// Interaction logic for UserEditor.xaml
  /// </summary>
  public partial class UserEditor : UserControl
  {
    LobbyVM vm;
    
    public UserEditor()
    {
      InitializeComponent();
      sign.EndEditCommand = new SimpleCommand(RefreshState);
    }

    internal void Init(LobbyVM lobby)
    {
      if (lobby != null)
      {
        vm = lobby;
        DataContext = vm.User;
      }
      else
      {
        vm = null;
        DataContext = null;
      }
    }

    void RefreshState(UserState state)
    {
      if (vm != null) PBOClient.Client.ChangeUserState(state, sign.Text);
    }
    void RefreshState()
    {
      RefreshState(vm.User.State);
    }

    private void normal_Click(object sender, RoutedEventArgs e)
    {
      RefreshState(UserState.Normal);
    }
    private void afk_Click(object sender, RoutedEventArgs e)
    {
      RefreshState(UserState.Afk);
    }
    private void aggressive_Click(object sender, RoutedEventArgs e)
    {
      RefreshState(UserState.Aggressive);
    }
    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
      sign.IsEditing = true;
    }
  }
}
