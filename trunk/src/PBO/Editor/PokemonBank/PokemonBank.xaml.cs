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

namespace PokemonBattleOnline.PBO.Editor
{
  /// <summary>
  /// Interaction logic for PokemonBank.xaml
  /// </summary>
  public partial class PokemonBank : UserControl
  {
    public PokemonBank()
    {
      InitializeComponent();
    }

    private void NewTeam_Click(object sender, RoutedEventArgs e)
    {
      EditorVM.Current.NewTeam();
    }
  }
}
