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
using LightStudio.PokemonBattle.Messaging.Room;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.PBO.Lobby
{
  /// <summary>
  /// Interaction logic for StartBattle.xaml
  /// </summary>
  public partial class StartBattle : Window
  {
    StartBattleVM vm;
    Point lastPosition;

    /// <param name="settings">主动的话这个不应该是null么</param>
    public StartBattle(User rival, GameInitSettings settings, bool isPassitive)
    {
      InitializeComponent();
      DataContext = vm = new StartBattleVM(rival, settings, isPassitive);
      vm.Processed += () => Close();
    }

    private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      lastPosition = e.GetPosition(this);
    }
    private void TextBlock_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        var p = e.GetPosition(this);
        Left += (p.X - lastPosition.X);
        Top += (p.Y - lastPosition.Y);
      }
    }
  }
}
