using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Reflection;

namespace PokemonBattleOnline.PBO.UIElements.Interactivity
{
  public class SyncSelectionAction : TriggerAction<Selector>
  {
    // Fields
    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(Selector), typeof(SyncSelectionAction), new UIPropertyMetadata(null));

    // Methods
    protected override void Invoke(object parameter)
    {
      if (this.Target != null)
      {
        object selectedItem = base.AssociatedObject.SelectedItem;
        if (this.Converter != null)
        {
          selectedItem = this.Converter.Convert(selectedItem, null, null, null);
        }
        this.Target.SelectedItem = selectedItem;
        if (this.Target.SelectedIndex != -1)
        {
          object obj3 = this.Target.ItemContainerGenerator.ContainerFromIndex(this.Target.SelectedIndex);
          if (obj3 != null)
          {
            if (obj3 is FrameworkElement)
            {
              (obj3 as FrameworkElement).BringIntoView();
            }
          }
          else
          {
            VirtualizingStackPanel panel = typeof(ItemsControl).InvokeMember("_itemsHost", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, this.Target, null) as VirtualizingStackPanel;
            if (panel != null)
            {
              double offset = (panel.ScrollOwner.ScrollableHeight * this.Target.SelectedIndex) / ((double)this.Target.Items.Count);
              panel.SetVerticalOffset(offset);
            }
          }
        }
      }
    }

    // Properties
    public IValueConverter Converter { get; set; }

    public Selector Target
    {
      get
      {
        return (Selector)base.GetValue(TargetProperty);
      }
      set
      {
        base.SetValue(TargetProperty, value);
      }
    }
  }
}
