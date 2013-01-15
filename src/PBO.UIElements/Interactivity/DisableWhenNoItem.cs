using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace PokemonBattleOnline.PBO.UIElements.Interactivity
{
  public class DisableWhenNoItem : Behavior<ContextMenu>
  {
    // Methods
    private void AssociatedObject_Opened(object sender, RoutedEventArgs e)
    {
      if (base.AssociatedObject.Items.Count == 0)
      {
        base.AssociatedObject.IsOpen = false;
      }
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      base.AssociatedObject.Opened += new RoutedEventHandler(this.AssociatedObject_Opened);
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      base.AssociatedObject.Opened -= new RoutedEventHandler(this.AssociatedObject_Opened);
    }
  }
}
