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
    public event Action<SimPokemon> ReviewPokemon = delegate { };
    ControlPanelVM vm;

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
      //if (vm != null)
      //  switch (vm.SelectedPanel)
      //  {
      //    case ControlPanelVM.INACTIVE:
      //      bg.Waiting();
      //      break;
      //    case ControlPanelVM.MAIN:
      //      bg.Menu();
      //      break;
      //    case ControlPanelVM.POKEMONS:
      //      bg.Pokemons();
      //      break;
      //    case ControlPanelVM.STOP:
      //      bg.Inner();
      //      break;
      //    default:
      //      bg.Inner();
      //      break;
      //  }
    }
    private void Return_Click(object sender, RoutedEventArgs e)
    {
      if (controlPanel.SelectedIndex == ControlPanelVM.TARGET)
        controlPanel.SelectedIndex = ControlPanelVM.FIGHT;
      controlPanel.SelectedIndex = ControlPanelVM.MAIN;
      Current = null;
    }
    private void Fight_Click(object sender, RoutedEventArgs e)
    {
      vm.Fight_Click();
    }
    private void Pokemons_Click(object sender, RoutedEventArgs e)
    {
      controlPanel.SelectedIndex = ControlPanelVM.POKEMONS;
    }
    private void Stop_Click(object sender, RoutedEventArgs e)
    {
      controlPanel.SelectedIndex = ControlPanelVM.STOP;
    }
    private void Move_Click(object sender, RoutedEventArgs e)
    {
      vm.Move_Click((SimMove)((GameButton)sender).Content);
    }
    private void Pokemon_Click(object sender, RoutedEventArgs e)
    {
      SimPokemon pm = (SimPokemon)((GameButton)sender).Content;
      if (pm == Current)
      {
        vm.Pokemon_Click(pm);
        Current = null;
      }
      else Current = pm;
    }
    private void Giveup_Click(object sender, RoutedEventArgs e)
    {
      vm.GiveUp_Click();
    }
    internal void Init(ControlPanelVM cp)
    {
      DataContext = vm = cp;
    }
  }
}
