using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using Adorner = System.Windows.Documents.Adorner;

namespace LightStudio.PokemonBattle.PBO.UIElements.Interactivity
{
  public class UIElementAdorner : Adorner
  {
    // Fields
    public static readonly DependencyProperty AdornmentProperty = DependencyProperty.Register("Adornment", typeof(UIElement), typeof(UIElementAdorner), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(UIElementAdorner.AdornmentChanged)));

    // Methods
    public UIElementAdorner(UIElement adornedElement)
      : base(adornedElement)
    {
      base.IsHitTestVisible = false;
    }

    private static void AdornmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      UIElementAdorner adorner = d as UIElementAdorner;
      if (e.OldValue != null)
      {
        adorner.RemoveLogicalChild(e.OldValue as UIElement);
      }
      if (e.NewValue != null)
      {
        adorner.AddLogicalChild(e.NewValue as UIElement);
      }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      if (this.Adornment != null)
      {
        this.Adornment.Arrange(new Rect(finalSize));
      }
      return base.ArrangeOverride(finalSize);
    }

    protected override Visual GetVisualChild(int index)
    {
      return this.Adornment;
    }

    protected override Size MeasureOverride(Size constraint)
    {
      if (this.Adornment != null)
      {
        this.Adornment.Measure(constraint);
      }
      return base.MeasureOverride(constraint);
    }

    // Properties
    public UIElement Adornment
    {
      get
      {
        return (UIElement)base.GetValue(AdornmentProperty);
      }
      set
      {
        base.SetValue(AdornmentProperty, value);
      }
    }

    protected override int VisualChildrenCount
    {
      get
      {
        return 1;
      }
    }
  }


}
