using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interactivity;

namespace PokemonBattleOnline.PBO.UIElements.Interactivity
{
  public class AdornerBehavior : Behavior<FrameworkElement>, IWeakEventListener
  {
    // Fields
    private AdornerLayer adornerLayer;
    public static readonly DependencyProperty AdornersProperty = DependencyProperty.Register("Adorners", typeof(FreezableCollection<AdornerProxy>), typeof(AdornerBehavior), new UIPropertyMetadata(null));

    // Methods
    public AdornerBehavior()
    {
      this.Adorners = new FreezableCollection<AdornerProxy>();
    }

    private void AddAdorners(IEnumerable<AdornerProxy> newAdorners)
    {
      foreach (AdornerProxy proxy in newAdorners)
      {
        if (proxy.IsAdornerCreated || proxy.CreateAdorner(AssociatedObject))
        {
          this.adornerLayer.Add(proxy.Adorner);
        }
      }
      this.adornerLayer.Update(AssociatedObject);
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
      this.adornerLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);
      CollectionChangedEventManager.AddListener(this.Adorners, this);
      this.AddAdorners(this.Adorners);
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      base.AssociatedObject.Loaded += new RoutedEventHandler(this.AssociatedObject_Loaded);
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      base.AssociatedObject.Loaded -= new RoutedEventHandler(this.AssociatedObject_Loaded);
      this.RemoveAdorners(this.Adorners);
      this.adornerLayer = null;
    }

    private void RemoveAdorners(IEnumerable<AdornerProxy> oldAdorners)
    {
      foreach (AdornerProxy proxy in oldAdorners)
      {
        if (proxy.IsAdornerCreated)
        {
          this.adornerLayer.Remove(proxy.Adorner);
        }
      }
      this.adornerLayer.Update(base.AssociatedObject);
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      if ((managerType == typeof(CollectionChangedEventManager)) && (sender == this.Adorners))
      {
        NotifyCollectionChangedEventArgs args = e as NotifyCollectionChangedEventArgs;
        if (args.NewItems != null)
        {
          this.AddAdorners(args.NewItems.Cast<AdornerProxy>());
        }
        if (args.OldItems != null)
        {
          this.RemoveAdorners(args.OldItems.Cast<AdornerProxy>());
        }
        return true;
      }
      return false;
    }

    // Properties
    public FreezableCollection<AdornerProxy> Adorners
    {
      get
      {
        return (FreezableCollection<AdornerProxy>)base.GetValue(AdornersProperty);
      }
      set
      {
        base.SetValue(AdornersProperty, value);
      }
    }
  }
}
