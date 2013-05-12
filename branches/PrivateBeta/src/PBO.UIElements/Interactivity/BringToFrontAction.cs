using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace LightStudio.PokemonBattle.PBO.UIElements.Interactivity
{
  public class BringToFrontAction : TriggerAction<UIElement>
  {
    // Fields
    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(UIElement), typeof(BringToFrontAction), new UIPropertyMetadata(null));

    // Methods
    protected override void Invoke(object parameter)
    {
      if (this.Target != null)
      {
        this.Target.BringToFront();
      }
      else
      {
        base.AssociatedObject.BringToFront();
      }
    }

    // Properties
    public UIElement Target
    {
      get
      {
        return (UIElement)base.GetValue(TargetProperty);
      }
      set
      {
        base.SetValue(TargetProperty, value);
      }
    }
  }
  internal static class PanelHelper
  {
    // Methods
    public static void BringToFront(this UIElement uiElement)
    {
      Panel parent = VisualTreeHelper.GetParent(uiElement) as Panel;
      if (parent != null)
      {
        IEnumerable<int> source = from element in parent.Children.OfType<UIElement>()
                                  where element != uiElement
                                  select Panel.GetZIndex(element);
        if (source.Any<int>())
        {
          int num = source.Max();
          if (num >= Panel.GetZIndex(uiElement))
          {
            Panel.SetZIndex(uiElement, num + 1);
          }
        }
      }
    }
  }
}
