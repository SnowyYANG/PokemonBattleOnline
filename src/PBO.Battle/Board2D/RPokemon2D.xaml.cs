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

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for Pokemon2D.xaml
  /// </summary>
  public partial class RPokemon2D : Canvas, IPokemonOutwardEvents
  {
    PokemonOutward pokemon;
    
    public RPokemon2D()
    {
      InitializeComponent();
    }

    public void Sendout(PokemonOutward pm)
    {
      pokemon = pm;
      if (pokemon != null)
      {
        pokemon.AddListener(this);
        if (pokemon.Gender == PokemonGender.Female)
          main.Source = DataService.Image.GetPokemonFemaleFront(pokemon.ImageId);
        else main.Source = DataService.Image.GetPokemonMaleFront(pokemon.ImageId);
      }
    }
    void IPokemonOutwardEvents.Faint()
    {
    }
    void IPokemonOutwardEvents.Hurt()
    {
    }
    void IPokemonOutwardEvents.PositionChanged()
    {
    }
    void IPokemonOutwardEvents.UseItem() //褐色光圈
    {
    }
    void IPokemonOutwardEvents.UseMove(int moveType)
    {
    }
    void IPokemonOutwardEvents.HpRecovered() //绿色光上升
    {
    }
    void IPokemonOutwardEvents.Lv5DUp() //绿色上升
    {
    }
    void IPokemonOutwardEvents.Lv5DDown() //下降
    {
    }
    void IPokemonOutwardEvents.SubstituteAppear()
    {
    }
    void IPokemonOutwardEvents.SubstituteDisappear()
    {
    }
    void IPokemonOutwardEvents.ImageIdChanged()
    {
    }
    void IPokemonOutwardEvents.Withdrawn()
    {
      pokemon.RemoveListener(this);
      pokemon = null;
    }
  }
}
