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
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Battle
{
  /// <summary>
  /// Interaction logic for ControlPanel.xaml
  /// </summary>
  public partial class ControlPanel : Canvas
  {
    public event Action<SimPokemon> ReviewPokemon;
    IControlPanel vm;

    public ControlPanel()
    {
      InitializeComponent();
    }

    private SimPokemon _current;
    private SimPokemon Current
    {
      get { return _current; }
      set
      {
        _current = value;
        ReviewPokemon(_current);
      }
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
      Current = null;
    }
    private void fight_Click(object sender, RoutedEventArgs e)
    {
      vm.Fight_Click();
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
      vm.Move_Click((SimMove)((Button)sender).Content);
    }
    private void pokemon_Click(object sender, RoutedEventArgs e)
    {
      SimPokemon pm = (SimPokemon)((Button)sender).Content;
      if (pm == Current)
      {
        vm.Pokemon_Click(pm);
        Current = null;
      }
      else Current = pm;
    }
    private void giveup_Click(object sender, RoutedEventArgs e)
    {
      var result = ShowMessageBox.GiveUpBattle(Window.GetWindow(this));
      if (result == MessageBoxResult.Yes) vm.GiveUp_Click();
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
