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
    protected readonly int Scale;
    protected PokemonOutward pokemon;
    private DoubleAnimation faint;

    protected Pokemon2DBase(int scale)
    {
      Scale = scale;
      main = new Image() { Stretch = Stretch.UniformToFill };
      main.SetValue(Canvas.BottomProperty, (double)-48 * Scale);
      Children.Add(main);
      IsHitTestVisible = false;
      SnapsToDevicePixels = true;
      faint = new DoubleAnimation(96 * scale, 0, Duration.Automatic);
      faint.Completed += (sender, e) =>
        {
          main.Source = null;
          pokemon.RemoveListener(this);
          pokemon = null;
          main.BeginAnimation(Image.HeightProperty, null);
        };
    }

    protected abstract BitmapImage GetImage(PokemonForm form, PokemonGender gender, bool shiny);
    protected abstract BitmapImage GetSp(string id);

    private void RefreshImage()
    {
      if (pokemon.IsSubstitute) main.Source = GetSp("substitute");
      else
      {
        var s = GetImage(pokemon.Form, pokemon.Gender, pokemon.Shiny);
        main.Source = s;
        main.SetValue(Canvas.LeftProperty, (double)-((s.PixelWidth * Scale) >> 1));
        main.Width = s.PixelWidth * Scale;
      }
    }
    public void Sendout(PokemonOutward pm)
    {
      pokemon = pm;
      if (pokemon != null)
      {
        pokemon.AddListener(this);
        RefreshImage();
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
      main.Visibility = pokemon.Position.Y == CoordY.Plate ? Visibility.Visible : Visibility.Collapsed;
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
      RefreshImage();
    }
    void IPokemonOutwardEvents.SubstituteDisappear()
    {
      RefreshImage();
    }
    void IPokemonOutwardEvents.FormChanged()
    {
      RefreshImage();
    }
    void IPokemonOutwardEvents.Withdrawn()
    {
      main.Source = null;
      pokemon.RemoveListener(this);
      pokemon = null;
    }
  }
}
