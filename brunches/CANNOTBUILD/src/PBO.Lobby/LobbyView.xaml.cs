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
using PokemonBattleOnline.Network;
using PokemonBattleOnline.PBO.UIElements;

namespace PokemonBattleOnline.PBO.Lobby
{
  /// <summary>
  /// Interaction logic for LobbyView.xaml
  /// </summary>
  public partial class LobbyView : UserControl
  {
    private LobbyVM vm;
    
    public LobbyView()
    {
      InitializeComponent();
      PBOClient.CurrentChanged += PBOClient_CurrentChanged;
    }

    internal void Init(LobbyVM lobby)
    {
      //if (lobby != null)
      //{
      //  vm = lobby;
      //  DataContext = vm;
      //  //PBOClient.Current.Challenged += (user, settings) =>
      //  //  {
      //  //    UIDispatcher.Invoke(()=>
      //  //      new StartBattle(user, settings, true).Show());
      //  //  };
      //  chat.Init();
      //}
      //else //uninit
      //{
      //  vm = null;
      //  DataContext = null;
      //}
    }

    private void PBOClient_CurrentChanged()
    {
      //already in lock
      if (PBOClient.Current == null) MessageBox.Show("CurrentChanged.null");
      else MessageBox.Show("CurrentChanged");
    }

    private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (ActualWidth > 500)
      {
        Grid.SetRowSpan(upper, 2);
        Grid.SetColumnSpan(upper, 1);
        Grid.SetRow(chat, 0);
        Grid.SetRowSpan(chat, 2);
        Grid.SetColumn(chat, 1);
        Grid.SetColumnSpan(chat, 1);
        Grid.SetColumnSpan(split, 1);
        Grid.SetRowSpan(split, 2);
        upper.Margin = new Thickness(0, 0, 3, 0);
        split.Cursor = Cursors.SizeWE;
        split.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
        split.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
      }
      else
      {
        Grid.SetRowSpan(upper, 1);
        Grid.SetColumnSpan(upper, 2);
        Grid.SetRow(chat, 1);
        Grid.SetRowSpan(chat, 1);
        Grid.SetColumn(chat, 0);
        Grid.SetColumnSpan(chat, 2);
        Grid.SetColumnSpan(split, 2);
        Grid.SetRowSpan(split, 1);
        upper.Margin = new Thickness(0, 0, 0, 3);
        split.Cursor = Cursors.SizeNS;
        split.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        split.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
      }
    }
    internal bool Window_Closing()
    {
      throw new NotImplementedException();
      //return PBOClient.Current != null && ShowMessageBox.ExitLobby() == MessageBoxResult.No;
    }
  }
}
