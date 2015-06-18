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
        ControlPanelVM vm;

        public ControlPanel()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Current.Content = null;
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            if (controlPanel.SelectedIndex == ControlPanelVM.TARGET) controlPanel.SelectedIndex = ControlPanelVM.FIGHT;
            else
            {
                vm.Mega = false;
                controlPanel.SelectedIndex = ControlPanelVM.MAIN;
                Current.Content = null;
            }
        }
        private void Fight_Click(object sender, RoutedEventArgs e)
        {
            vm.Fight_Click();
        }
        private void Pokemons_Click(object sender, RoutedEventArgs e)
        {
            controlPanel.SelectedIndex = ControlPanelVM.POKEMONS;
        }
        private void Run_Click(object sender, RoutedEventArgs e)
        {
            controlPanel.SelectedIndex = ControlPanelVM.RUN;
        }
        private void Move_Click(object sender, RoutedEventArgs e)
        {
            vm.Move_Click((SimMove)((GameButton)sender).Content);
        }
        private void Pokemon_Click(object sender, RoutedEventArgs e)
        {
            SimPokemon pm = (SimPokemon)((GameButton)sender).Content;
            if (pm == Current.Content)
            {
                vm.Pokemon_Click(pm);
                Current.Content = null;
            }
            else
            {
                Current.Content = pm;
            }
        }
        private void GiveUp_Click(object sender, RoutedEventArgs e)
        {
            vm.GiveUp_Click();
        }
        internal void Init(ControlPanelVM cp)
        {
            DataContext = vm = cp;
            Current.Content = null;
        }

        internal void Reset()
        {
            Current.Content = null;
        }
    }
}
