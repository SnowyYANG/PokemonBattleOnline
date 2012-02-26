using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using Adorner = System.Windows.Documents.Adorner;

namespace LightStudio.PokemonBattle.PBO.UIElements.Interactivity
{
  public class DropHighlightAdorner : Adorner
  {
    // Fields
    public static readonly DependencyProperty FillProperty = Shape.FillProperty.AddOwner(typeof(DropHighlightAdorner));
    public static readonly DependencyProperty HighlightIndexProperty = DependencyProperty.Register("HighlightIndex", typeof(int), typeof(DropHighlightAdorner), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty PaddingProperty = Control.PaddingProperty.AddOwner(typeof(DropHighlightAdorner));
    public static readonly DependencyProperty PenProperty = DependencyProperty.Register("Pen", typeof(Pen), typeof(DropHighlightAdorner), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty RadiusXProperty = Rectangle.RadiusXProperty.AddOwner(typeof(DropHighlightAdorner));
    public static readonly DependencyProperty RadiusYProperty = Rectangle.RadiusYProperty.AddOwner(typeof(DropHighlightAdorner));

    // Methods
    public DropHighlightAdorner(UIElement adornedElement)
      : base(adornedElement)
    {
      base.IsHitTestVisible = false;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);
      if ((this.Pen != null) && (0 == 0))
      {
        ItemsControl adornedElement = base.AdornedElement as ItemsControl;
        if ((adornedElement != null) && ((this.HighlightIndex < adornedElement.Items.Count) && (this.HighlightIndex >= 0)))
        {
          FrameworkElement element = adornedElement.ItemContainerGenerator.ContainerFromIndex(this.HighlightIndex) as FrameworkElement;
          if (element != null)
          {
            element.BringIntoView();
            Rect rectangle = new Rect(element.TranslatePoint(new Point(this.Padding.Left, this.Padding.Top), adornedElement), new Size((element.RenderSize.Width + this.Padding.Left) + this.Padding.Right, (element.RenderSize.Height + this.Padding.Top) + this.Padding.Bottom));
            drawingContext.DrawRoundedRectangle(this.Fill, this.Pen, rectangle, this.RadiusX, this.RadiusY);
          }
        }
      }
    }

    // Properties
    public Brush Fill
    {
      get
      {
        return (Brush)base.GetValue(FillProperty);
      }
      set
      {
        base.SetValue(FillProperty, value);
      }
    }

    public int HighlightIndex
    {
      get
      {
        return (int)base.GetValue(HighlightIndexProperty);
      }
      set
      {
        base.SetValue(HighlightIndexProperty, value);
      }
    }

    public Thickness Padding
    {
      get
      {
        return (Thickness)base.GetValue(PaddingProperty);
      }
      set
      {
        base.SetValue(PaddingProperty, value);
      }
    }

    public Pen Pen
    {
      get
      {
        return (Pen)base.GetValue(PenProperty);
      }
      set
      {
        base.SetValue(PenProperty, value);
      }
    }

    public double RadiusX
    {
      get
      {
        return (double)base.GetValue(RadiusXProperty);
      }
      set
      {
        base.SetValue(RadiusXProperty, value);
      }
    }

    public double RadiusY
    {
      get
      {
        return (double)base.GetValue(RadiusYProperty);
      }
      set
      {
        base.SetValue(RadiusYProperty, value);
      }
    }
  }


}
