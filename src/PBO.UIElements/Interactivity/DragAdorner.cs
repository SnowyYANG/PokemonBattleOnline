using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Documents;

namespace LightStudio.PokemonBattle.PBO.UIElements.Interactivity
{
  public class DragAdorner : Adorner, IWeakEventListener
  {
    // Fields
    private AdornerLayer adornerLayer;
    private ContentControl adornment;
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(DragAdorner), new UIPropertyMetadata(null, new PropertyChangedCallback(DragAdorner.ContentChanged)));
    public static readonly DependencyProperty ContentTemplateProperty = ContentPresenter.ContentTemplateProperty.AddOwner(typeof(DragAdorner));

    // Methods
    public DragAdorner(UIElement adornedElement)
      : base(adornedElement)
    {
      this.adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
      this.InitializeAdornment();
      DependencyPropertyDescriptor.FromProperty(DragDropState.MousePositionPropertyKey.DependencyProperty, base.AdornedElement.GetType()).AddValueChanged(base.AdornedElement, new EventHandler(this.MousePositionChanged));
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      this.adornment.Arrange(new Rect(finalSize));
      return base.ArrangeOverride(finalSize);
    }

    private static void ContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DragAdorner listener = d as DragAdorner;
      INotifyPropertyChanged oldValue = e.OldValue as INotifyPropertyChanged;
      if (oldValue != null)
      {
        PropertyChangedEventManager.RemoveListener(oldValue, listener, string.Empty);
      }
      INotifyPropertyChanged newValue = e.NewValue as INotifyPropertyChanged;
      if (newValue != null)
      {
        PropertyChangedEventManager.AddListener(newValue, listener, string.Empty);
      }
      listener.ReRender();
    }

    protected override Visual GetVisualChild(int index)
    {
      return this.adornment;
    }

    private void InitializeAdornment()
    {
      this.adornment = new ContentControl();
      this.adornment.HorizontalAlignment = HorizontalAlignment.Left;
      this.adornment.VerticalAlignment = VerticalAlignment.Top;
      Binding binding = new Binding("Content")
      {
        Source = this
      };
      this.adornment.SetBinding(ContentPresenter.ContentProperty, binding);
      Binding binding2 = new Binding("ContentTemplate")
      {
        Source = this
      };
      this.adornment.SetBinding(ContentPresenter.ContentTemplateProperty, binding2);
    }

    protected override Size MeasureOverride(Size constraint)
    {
      this.adornment.Measure(constraint);
      return base.MeasureOverride(constraint);
    }

    private void MousePositionChanged(object sender, EventArgs e)
    {
      Point mousePosition = DragDropState.GetMousePosition(base.AdornedElement);
      this.adornment.RenderTransform = new TranslateTransform(mousePosition.X, mousePosition.Y);
      this.ReRender();
    }

    private void ReRender()
    {
      this.adornerLayer.Update(base.AdornedElement);
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      if (managerType == typeof(PropertyChangedEventManager))
      {
        this.ReRender();
        return true;
      }
      return false;
    }

    // Properties
    public object Content
    {
      get
      {
        return base.GetValue(ContentProperty);
      }
      set
      {
        base.SetValue(ContentProperty, value);
      }
    }

    public DataTemplate ContentTemplate
    {
      get
      {
        return (DataTemplate)base.GetValue(ContentTemplateProperty);
      }
      set
      {
        base.SetValue(ContentTemplateProperty, value);
      }
    }

    protected override int VisualChildrenCount
    {
      get
      {
        return 1;
      }
    }
  }


}
