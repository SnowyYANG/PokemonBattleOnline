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
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.PBO.Battle.VM;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for ControlPanel.xaml
  /// </summary>
  public partial class ControlPanel : Canvas
  {
    IControlPanel vm;

    public ControlPanel()
    {
      InitializeComponent();
    }
    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (vm != null)
        switch (vm.SelectedPanel)
        {
          case ControlPanelIndex.INACTIVE:
            bg.Waiting();
            break;
          case ControlPanelIndex.MAIN:
            bg.Menu();
            break;
          case ControlPanelIndex.POKEMONS:
            bg.Pokemons();
            break;
          case ControlPanelIndex.STOP:
            bg.Inner();
            break;
          default:
            bg.Inner();
            break;
        }
    }
    private void return_Click(object sender, RoutedEventArgs e)
    {
      if (controlPanel.SelectedIndex == ControlPanelIndex.TARGET)
        controlPanel.SelectedIndex = ControlPanelIndex.FIGHT;
      controlPanel.SelectedIndex = ControlPanelIndex.MAIN;
    }
    private void fight_Click(object sender, RoutedEventArgs e)
    {
      controlPanel.SelectedIndex = ControlPanelIndex.FIGHT;
    }
    private void pokemons_Click(object sender, RoutedEventArgs e)
    {
      controlPanel.SelectedIndex = ControlPanelIndex.POKEMONS;
    }
    private void stop_Click(object sender, RoutedEventArgs e)
    {
      controlPanel.SelectedIndex = ControlPanelIndex.STOP;
    }
    private void move_Click(object sender, RoutedEventArgs e)
    {
      Move move = ((Button)sender).Content as Move;
      if (move != null) vm.Move_Click(move);
    }
    private void pokemon_Click(object sender, RoutedEventArgs e)
    {
      Pokemon pm = ((Button)sender).Content as Pokemon;
      if (pm != null) vm.Pokemon_Click(pm);
    }
    private void giveup_Click(object sender, RoutedEventArgs e)
    {
      var result = UIElements.ShowMessageBox.GiveUpBattle(Window.GetWindow(this));
      if (result == MessageBoxResult.Yes) vm.Giveup_Click();
    }
    private void draw_Click(object sender, RoutedEventArgs e)
    {
      System.Windows.MessageBox.Show("请期待下一版本...");
    }
    internal void Init(IControlPanel controlpanel)
    {
      if (DataContext != null) throw new Exception("<Testbug>ControlPanel inits twice...");
      DataContext = vm = controlpanel;
    }
  }
}
