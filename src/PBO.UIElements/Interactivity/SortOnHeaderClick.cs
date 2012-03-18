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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace LightStudio.PokemonBattle.PBO.UIElements.Interactivity
{
  public class SortOnHeaderClick : Behavior<ListView>
  {
    // Fields
    public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.RegisterAttached("SortDirection", typeof(ListSortDirection?), typeof(SortOnHeaderClick), new UIPropertyMetadata(null));
    private GridViewColumnHeader sortingHeader;
    public static readonly DependencyProperty SortPropertyProperty = DependencyProperty.RegisterAttached("SortProperty", typeof(string), typeof(SortOnHeaderClick), new UIPropertyMetadata(null));

    // Methods
    public static ListSortDirection? GetSortDirection(DependencyObject obj)
    {
      return (ListSortDirection?)obj.GetValue(SortDirectionProperty);
    }

    public static string GetSortProperty(DependencyObject obj)
    {
      return (string)obj.GetValue(SortPropertyProperty);
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      base.AssociatedObject.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnHeaderClicked));
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      base.AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnHeaderClicked));
    }

    private void OnHeaderClicked(object sender, RoutedEventArgs e)
    {
      GridViewColumnHeader originalSource = e.OriginalSource as GridViewColumnHeader;
      if (originalSource != null)
      {
        GridViewColumn column = originalSource.Column;
        if (column != null)
        {
          string sortProperty = GetSortProperty(column);
          if (sortProperty == null)
          {
            Binding displayMemberBinding = column.DisplayMemberBinding as Binding;
            if (displayMemberBinding != null)
            {
              sortProperty = displayMemberBinding.Path.Path;
            }
          }
          if (sortProperty != null)
          {
            ListSortDirection ascending = ListSortDirection.Ascending;
            if (this.sortingHeader == originalSource)
            {
              if (((ListSortDirection)GetSortDirection(this.sortingHeader).Value) == ListSortDirection.Ascending)
              {
                ascending = ListSortDirection.Descending;
              }
              else
              {
                ascending = ListSortDirection.Ascending;
              }
            }
            else
            {
              if (this.sortingHeader != null)
              {
                SetSortDirection(this.sortingHeader, null);
              }
              this.sortingHeader = originalSource;
            }
            ICollectionView defaultView = CollectionViewSource.GetDefaultView(base.AssociatedObject.ItemsSource);
            defaultView.SortDescriptions.Clear();
            defaultView.SortDescriptions.Add(new SortDescription(sortProperty, ascending));
            SetSortDirection(this.sortingHeader, new ListSortDirection?(ascending));
          }
        }
      }
    }

    private static void SetSortDirection(DependencyObject obj, ListSortDirection? value)
    {
      obj.SetValue(SortDirectionProperty, value);
    }

    public static void SetSortProperty(DependencyObject obj, string value)
    {
      obj.SetValue(SortPropertyProperty, value);
    }
  }
}
