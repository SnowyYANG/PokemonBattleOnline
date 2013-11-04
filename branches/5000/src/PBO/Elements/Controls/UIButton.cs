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

namespace PokemonBattleOnline.PBO.Elements
{
  public class UIButton : Button
  {
    static UIButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(UIButton), new FrameworkPropertyMetadata(typeof(UIButton)));
    }
    #region Shape
    public static readonly DependencyProperty ShapeProperty = DependencyProperty.Register("Shape", typeof(Geometry), typeof(UIButton));
    public Geometry Shape
    {
      get { return (Geometry)GetValue(ShapeProperty); }
      set { SetValue(ShapeProperty, value); }
    }
    #endregion

    #region Shininess
    public static readonly DependencyProperty ShininessProperty = DependencyProperty.Register("Shininess", typeof(Brush), typeof(UIButton));
    public Brush Shininess
    {
      get { return (Brush)GetValue(ShininessProperty); }
      set { SetValue(ShininessProperty, value); }
    }
    #endregion

    #region HighLight Brushes
    public static readonly DependencyProperty BackgroundHLProperty = DependencyProperty.Register("BackgroundHL", typeof(Brush), typeof(UIButton));
    public Brush BackgroundHL
    {
      get { return (Brush)GetValue(BackgroundHLProperty); }
      set { SetValue(BackgroundHLProperty, value); }
    }

    public static readonly DependencyProperty BorderBrushHLProperty = DependencyProperty.Register("BorderBrushHL", typeof(Brush), typeof(UIButton));
    public Brush BorderBrushHL
    {
      get { return (Brush)GetValue(BorderBrushHLProperty); }
      set { SetValue(BorderBrushHLProperty, value); }
    }
    #endregion
  }
}
