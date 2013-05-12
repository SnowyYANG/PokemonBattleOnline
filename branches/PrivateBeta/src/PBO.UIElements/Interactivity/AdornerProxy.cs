using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using Adorner = System.Windows.Documents.Adorner;

namespace LightStudio.PokemonBattle.PBO.UIElements.Interactivity
{
  public class AdornerProxy : DependencyObject
  {
    // Fields
    public static readonly DependencyProperty AdornerStyleProperty = DependencyProperty.Register("AdornerStyle", typeof(Style), typeof(AdornerProxy), new UIPropertyMetadata(null));

    // Methods
    public bool CreateAdorner(UIElement adornedElement)
    {
      if (this.AdornerType != null)
      {
        this.Adorner = Activator.CreateInstance(this.AdornerType, new object[] { adornedElement }) as Adorner;
        if (this.AdornerStyle != null)
        {
          this.Adorner.Style = this.AdornerStyle;
        }
        return true;
      }
      return false;
    }

    // Properties
    public Adorner Adorner { get; private set; }

    public Style AdornerStyle
    {
      get
      {
        return (Style)base.GetValue(AdornerStyleProperty);
      }
      set
      {
        base.SetValue(AdornerStyleProperty, value);
      }
    }

    public Type AdornerType { get; set; }

    public bool IsAdornerCreated
    {
      get
      {
        return (this.Adorner != null);
      }
    }
  }
}
