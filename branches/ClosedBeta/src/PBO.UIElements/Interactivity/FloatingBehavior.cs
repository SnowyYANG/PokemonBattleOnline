using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace PokemonBattleOnline.PBO.UIElements.Interactivity
{
  public class FloatingBehavior : Behavior<UIElement>
  {
    // Fields
    public static readonly DependencyProperty AutoBringToFrontProperty = DependencyProperty.Register("AutoBringToFront", typeof(bool), typeof(FloatingBehavior), new UIPropertyMetadata(true));
    private bool draging;
    public static readonly DependencyProperty FloatingElementProperty = DependencyProperty.RegisterAttached("FloatingElement", typeof(FrameworkElement), typeof(FloatingBehavior), new UIPropertyMetadata(null));
    private Point origin;

    // Methods
    private void AssociatedObject_GotMouseCapture(object sender, MouseEventArgs e)
    {
      this.draging = true;
      this.origin = e.GetPosition(base.AssociatedObject);
      if (this.AutoBringToFront)
      {
        FrameworkElement floatingElement = GetFloatingElement(base.AssociatedObject);
        if (floatingElement != null)
        {
          floatingElement.BringToFront();
        }
      }
    }

    private void AssociatedObject_LostMouseCapture(object sender, MouseEventArgs e)
    {
      this.draging = false;
    }

    private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Mouse.Capture(base.AssociatedObject);
    }

    private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      Mouse.Capture(null);
    }

    private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.draging)
      {
        FrameworkElement floatingElement = GetFloatingElement(base.AssociatedObject);
        if (floatingElement == null)
        {
          floatingElement = base.AssociatedObject as FrameworkElement;
        }
        if (floatingElement != null)
        {
          Canvas parent = VisualTreeHelper.GetParent(floatingElement) as Canvas;
          if (parent != null)
          {
            Vector vector = (Vector)(e.GetPosition(base.AssociatedObject) - this.origin);
            if (!(vector.Length == 0.0))
            {
              double num = Canvas.GetLeft(floatingElement) + vector.X;
              double num2 = Canvas.GetTop(floatingElement) + vector.Y;
              num = Correct(0.0, parent.ActualWidth - floatingElement.ActualWidth, num);
              num2 = Correct(0.0, parent.ActualHeight - floatingElement.ActualHeight, num2);
              Canvas.SetLeft(floatingElement, num);
              Canvas.SetTop(floatingElement, num2);
            }
          }
        }
      }
    }

    private static double Correct(double min, double max, double value)
    {
      if (double.IsNaN(value) || (value < min))
      {
        return min;
      }
      if (value > max)
      {
        return max;
      }
      return value;
    }

    public static FrameworkElement GetFloatingElement(DependencyObject obj)
    {
      return (FrameworkElement)obj.GetValue(FloatingElementProperty);
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonDown);
      AssociatedObject.MouseMove += new MouseEventHandler(this.AssociatedObject_MouseMove);
      AssociatedObject.LostMouseCapture += new MouseEventHandler(this.AssociatedObject_LostMouseCapture);
      AssociatedObject.GotMouseCapture += new MouseEventHandler(this.AssociatedObject_GotMouseCapture);
      AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonUp);
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
    }

    public static void SetFloatingElement(DependencyObject obj, FrameworkElement value)
    {
      obj.SetValue(FloatingElementProperty, value);
    }

    // Properties
    public bool AutoBringToFront
    {
      get
      {
        return (bool)base.GetValue(AutoBringToFrontProperty);
      }
      set
      {
        base.SetValue(AutoBringToFrontProperty, value);
      }
    }
  }


}
