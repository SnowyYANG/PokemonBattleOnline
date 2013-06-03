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

namespace PokemonDataEditor.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchMove_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string text = (sender as TextBox).Text;
                MoveList.SelectedValue = text;
                MoveList.ScrollIntoView(MoveList.SelectedItem);
            }
        }

        private void SearchPokemon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    string text = (sender as TextBox).Text;
                    PokemonList.SelectedValue = text;
                    PokemonList.ScrollIntoView(PokemonList.SelectedItem);
                }
                catch
                { }
            }
        }

    }
}
