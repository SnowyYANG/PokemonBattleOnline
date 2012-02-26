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
  public class DropInsertionAdorner : Adorner
  {
    // Fields
    public static readonly DependencyProperty InsertionIndexProperty = DependencyProperty.Register("InsertionIndex", typeof(int), typeof(DropInsertionAdorner), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset", typeof(Point), typeof(DropInsertionAdorner), new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(DropInsertionAdorner), new UIPropertyMetadata(Orientation.Horizontal));
    public static readonly DependencyProperty PenProperty = DependencyProperty.Register("Pen", typeof(Pen), typeof(DropInsertionAdorner), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    // Methods
    public DropInsertionAdorner(UIElement adornedElement)
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
        if ((adornedElement != null) && ((this.InsertionIndex <= adornedElement.Items.Count) && (this.InsertionIndex >= 0)))
        {
          FrameworkElement element;
          Point point;
          Point point2;
          if (this.InsertionIndex != adornedElement.Items.Count)
          {
            element = adornedElement.ItemContainerGenerator.ContainerFromIndex(this.InsertionIndex) as FrameworkElement;
          }
          else
          {
            element = adornedElement.ItemContainerGenerator.ContainerFromIndex(adornedElement.Items.Count - 1) as FrameworkElement;
          }
          Point offset = this.Offset;
          if (this.Orientation == Orientation.Horizontal)
          {
            if (element != null)
            {
              if (this.InsertionIndex == adornedElement.Items.Count)
              {
                offset = new Point(-offset.X + element.ActualWidth, offset.Y);
              }
              point = element.TranslatePoint(offset, adornedElement);
              point2 = new Point(point.X, point.Y + element.ActualHeight);
            }
            else
            {
              point = offset;
              point2 = new Point(point.X, point.Y + 50.0);
            }
          }
          else if (element != null)
          {
            if (this.InsertionIndex == adornedElement.Items.Count)
            {
              offset = new Point(offset.X, -offset.Y + element.ActualHeight);
            }
            point = element.TranslatePoint(offset, adornedElement);
            point2 = new Point(point.X + element.ActualWidth, point.Y);
          }
          else
          {
            point = offset;
            point2 = new Point(point.X + 50.0, point.Y);
          }
          Rect rect = new Rect(new Point(), adornedElement.RenderSize);
          if (rect.Contains(point) || rect.Contains(point2))
          {
            if (point2.X > rect.Width)
            {
              point2.X = rect.Width;
            }
            if (point2.Y > rect.Height)
            {
              point2.Y = rect.Height;
            }
            drawingContext.DrawLine(this.Pen, point, point2);
          }
        }
      }
    }

    // Properties
    public int InsertionIndex
    {
      get
      {
        return (int)base.GetValue(InsertionIndexProperty);
      }
      set
      {
        base.SetValue(InsertionIndexProperty, value);
      }
    }

    public Point Offset
    {
      get
      {
        return (Point)base.GetValue(OffsetProperty);
      }
      set
      {
        base.SetValue(OffsetProperty, value);
      }
    }

    public Orientation Orientation
    {
      get
      {
        return (Orientation)base.GetValue(OrientationProperty);
      }
      set
      {
        base.SetValue(OrientationProperty, value);
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
  }


}
