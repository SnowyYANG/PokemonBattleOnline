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
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Battle
{
  abstract class Pokemon2DBase : Canvas, IPokemonOutwardEvents
  {
    protected readonly Image main;
    protected readonly int Scale;
    protected PokemonOutward pokemon;
    private readonly DoubleAnimation faint;
    private readonly DoubleAnimation beginChangeImage;
    private readonly DoubleAnimation endChangeImage;
    private readonly BlurEffect imageChanging;

    protected Pokemon2DBase(int scale)
    {
      Scale = scale;
      main = new Image() { Stretch = Stretch.UniformToFill };
      main.SetValue(Canvas.BottomProperty, (double)-48 * Scale);
      main.Effect = imageChanging = new BlurEffect() { Radius = 0, KernelType = KernelType.Box };
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
      endChangeImage = new DoubleAnimation(15 * Scale, 0, new Duration(TimeSpan.FromSeconds(0.5)));
      beginChangeImage = new DoubleAnimation(0, 15 * Scale, new Duration(TimeSpan.FromSeconds(0.5)));
      beginChangeImage.Completed += (sender, e) =>
        {
          RefreshImage();
          imageChanging.BeginAnimation(BlurEffect.RadiusProperty, endChangeImage);
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
        main.Visibility = System.Windows.Visibility.Visible;
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
    void IPokemonOutwardEvents.SubstituteAppear()
    {
      RefreshImage();
    }
    void IPokemonOutwardEvents.SubstituteDisappear()
    {
      RefreshImage();
    }
    void IPokemonOutwardEvents.ImageChanged()
    {
      imageChanging.BeginAnimation(BlurEffect.RadiusProperty, beginChangeImage);
    }
    void IPokemonOutwardEvents.Withdrawn()
    {
      main.Source = null;
      pokemon.RemoveListener(this);
      pokemon = null;
    }
  }
}
