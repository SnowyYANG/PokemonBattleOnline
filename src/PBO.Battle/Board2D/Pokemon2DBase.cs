using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  abstract class Pokemon2DBase : Canvas, IPokemonOutwardEvents
  {
    protected readonly Image main;
    protected PokemonOutward pokemon;
    private DoubleAnimation faint;

    protected Pokemon2DBase(double size)
    {
      main = new Image() { Height = size, Width = size, Stretch = Stretch.UniformToFill };
      main.SetValue(Canvas.LeftProperty, size * -0.5);
      main.SetValue(Canvas.BottomProperty, size * -0.5);
      Children.Add(main);
      IsHitTestVisible = false;
      SnapsToDevicePixels = true;
      faint = new DoubleAnimation(size, 0, Duration.Automatic);
      faint.Completed += (sender, e) =>
        {
          main.Source = null;
          pokemon.RemoveListener(this);
          pokemon = null;
          main.BeginAnimation(Image.HeightProperty, null);
        };
    }

    protected abstract ImageSource GetPokemonFemale(int id);
    protected abstract ImageSource GetPokemonMale(int id);
    
    public void Sendout(PokemonOutward pm)
    {
      pokemon = pm;
      if (pokemon != null)
      {
        pokemon.AddListener(this);
        if (pokemon.Gender == PokemonGender.Female) main.Source = GetPokemonFemale(pokemon.ImageId);
        else main.Source = GetPokemonMale(pokemon.ImageId);
      }
    }
    void IPokemonOutwardEvents.Faint()
    {
      main.BeginAnimation(Image.HeightProperty, faint);
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
    void IPokemonOutwardEvents.Lv5DUp() //红色上升
    {
    }
    void IPokemonOutwardEvents.Lv5DDown() //绿色下降
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
      main.Source = null;
      pokemon.RemoveListener(this);
      pokemon = null;
    }
  }
}
