using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace PokemonBattleOnline.PBO.UIElements.Interactivity
{
  public class ResetValueOnBindingError : Behavior<FrameworkElement>
  {
    // Fields
    private DependencyPropertyDescriptor hasErrorProperty;

    // Methods
    private void HasError_Changed(object sender, EventArgs e)
    {
      if (this.Property != null)
      {
        BindingExpression bindingExpression = base.AssociatedObject.GetBindingExpression(this.Property);
        if (bindingExpression.HasError)
        {
          Application.Current.Dispatcher.BeginInvoke(new Action(bindingExpression.UpdateTarget), new object[0]);
        }
      }
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      this.hasErrorProperty = DependencyPropertyDescriptor.FromProperty(Validation.HasErrorProperty, base.AssociatedObject.GetType());
      this.hasErrorProperty.AddValueChanged(base.AssociatedObject, new EventHandler(this.HasError_Changed));
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      this.hasErrorProperty.RemoveValueChanged(base.AssociatedObject, new EventHandler(this.HasError_Changed));
    }

    // Properties
    public DependencyProperty Property { get; set; }
  }


}
