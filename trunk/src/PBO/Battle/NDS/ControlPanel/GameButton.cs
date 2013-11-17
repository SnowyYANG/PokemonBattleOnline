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

namespace PokemonBattleOnline.PBO.Battle
{
  public class GameButton : Button
  {
    static GameButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(GameButton), new FrameworkPropertyMetadata(typeof(GameButton)));
    }
    #region Shape
    public static readonly DependencyProperty ShapeProperty = DependencyProperty.Register("Shape", typeof(Geometry), typeof(GameButton));
    public Geometry Shape
    {
      get { return (Geometry)GetValue(ShapeProperty); }
      set { SetValue(ShapeProperty, value); }
    }
    #endregion
    #region Image
    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(GameButton));
    public ImageSource Image
    {
      get { return (ImageSource)GetValue(ImageProperty); }
      set { SetValue(ImageProperty, value); }
    }
    #endregion
    #region HorizontalFlip
    public static readonly DependencyProperty HorizontalFlipProperty = DependencyProperty.Register("HorizontalFlip", typeof(bool), typeof(GameButton));
    public bool HorizontalFlip
    {
      get { return (bool)GetValue(HorizontalFlipProperty); }
      set { SetValue(ImageProperty, value); }
    }
    #endregion
  }
}
