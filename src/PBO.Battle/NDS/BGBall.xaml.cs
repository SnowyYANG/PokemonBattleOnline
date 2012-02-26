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
using System.Windows.Media.Animation;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BGBall.xaml
  /// </summary>
  public partial class BGBall : UserControl
  {
    public BGBall()
    {
      InitializeComponent();
    }

    private void BeginStoryboard(string resourcename)
    {
      BeginStoryboard(Resources[resourcename] as Storyboard);
    }

    public void Waiting()
    {
      BeginStoryboard("Dark");
      BeginStoryboard("Sharp");
      BeginStoryboard("Far");
    }
    public void Menu()
    {
      BeginStoryboard("Light");
      BeginStoryboard("Sharp");
      BeginStoryboard("Near");
    }
    public void Inner()
    {
      BeginStoryboard("Shine");
      BeginStoryboard("Sharp");
      BeginStoryboard("Inner");
    }
    public void Pokemons()
    {
      BeginStoryboard("Dark");
      BeginStoryboard("Blur");
      BeginStoryboard("Near");
    }
  }
}
