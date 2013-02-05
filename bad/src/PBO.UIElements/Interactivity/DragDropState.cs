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
  public interface IDragDropData
  {
    // Properties
    object Data { get; }
    object Source { get; }
  }
  public interface IDragDropSource
  {
    // Methods
    void HandleGiveFeedback(UIElement sender, GiveFeedbackEventArgs e);
    void HandleMouseMove(UIElement sender, MouseEventArgs e);
    void HandleQueryContinueDrag(UIElement sender, QueryContinueDragEventArgs e);
  }
  public interface IDragDropTarget
  {
    // Methods
    void HandleDragEnter(UIElement sender, DragEventArgs e);
    void HandleDragLeave(UIElement sender, DragEventArgs e);
    void HandleDragOver(UIElement sender, DragEventArgs e);
    void HandleDrop(UIElement sender, DragEventArgs e);
  }
  public class DragDropState : Behavior<UIElement>
  {
    // Fields
    public static readonly DependencyProperty SourceHandlerProperty = DependencyProperty.Register("SourceHandler", typeof(IDragDropSource), typeof(DragDropState), new UIPropertyMetadata(null));
    public static readonly DependencyProperty TargetHandlerProperty = DependencyProperty.Register("TargetHandler", typeof(IDragDropTarget), typeof(DragDropState), new UIPropertyMetadata(null));
    public static readonly DependencyProperty DragDropDataProperty = DependencyProperty.RegisterAttached("DragDropData", typeof(IDragDropData), typeof(DragDropState), new UIPropertyMetadata(null));
    public static readonly DependencyProperty ItemOrientationProperty = DependencyProperty.RegisterAttached("ItemOrientation", typeof(Orientation), typeof(DragDropState), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty InsertionWidthProperty = DependencyProperty.Register("InsertionWidth", typeof(double), typeof(DragDropState), new UIPropertyMetadata(20.0));
    public static readonly DependencyPropertyKey IsDraggingPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsDragging", typeof(bool), typeof(DragDropState), new UIPropertyMetadata(false));
    public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;
    public static readonly DependencyPropertyKey InsertionIndexPropertyKey = DependencyProperty.RegisterAttachedReadOnly("InsertionIndex", typeof(int), typeof(DragDropState), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty InsertionIndexProperty = InsertionIndexPropertyKey.DependencyProperty;
    public static readonly DependencyPropertyKey MousePositionPropertyKey = DependencyProperty.RegisterAttachedReadOnly("MousePosition", typeof(Point), typeof(DragDropState), new UIPropertyMetadata(new Point()));
    public static readonly DependencyProperty MousePositionProperty = MousePositionPropertyKey.DependencyProperty;
    public static readonly DependencyPropertyKey DraggingOverIndexPropertyKey = DependencyProperty.RegisterAttachedReadOnly("DraggingOverIndex", typeof(int), typeof(DragDropState), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty DraggingOverIndexProperty = DraggingOverIndexPropertyKey.DependencyProperty;

    // Methods
    private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
    {
      IDragDropTarget targetHandler = this.TargetHandler;
      if (targetHandler != null)
      {
        targetHandler.HandleDragEnter(base.AssociatedObject, e);
      }
      this.StartDragDrop(e);
    }
    private void AssociatedObject_DragLeave(object sender, DragEventArgs e)
    {
      IDragDropTarget targetHandler = this.TargetHandler;
      if (targetHandler != null)
      {
        targetHandler.HandleDragLeave(base.AssociatedObject, e);
      }
      this.EndDragDrop(e);
    }
    private void AssociatedObject_DragOver(object sender, DragEventArgs e)
    {
      IDragDropTarget targetHandler = this.TargetHandler;
      if (targetHandler != null)
      {
        targetHandler.HandleDragOver(base.AssociatedObject, e);
      }
      this.UpdateState(e);
    }
    private void AssociatedObject_Drop(object sender, DragEventArgs e)
    {
      IDragDropTarget targetHandler = this.TargetHandler;
      if (targetHandler != null)
      {
        targetHandler.HandleDrop(base.AssociatedObject, e);
      }
      this.EndDragDrop(e);
    }
    private void AssociatedObject_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      IDragDropSource sourceHandler = this.SourceHandler;
      if (sourceHandler != null)
      {
        sourceHandler.HandleGiveFeedback(base.AssociatedObject, e);
      }
    }
    private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
    {
      IDragDropSource sourceHandler = this.SourceHandler;
      if (sourceHandler != null)
      {
        sourceHandler.HandleMouseMove(base.AssociatedObject, e);
      }
    }
    private void AssociatedObject_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      IDragDropSource sourceHandler = this.SourceHandler;
      if (sourceHandler != null)
      {
        sourceHandler.HandleQueryContinueDrag(base.AssociatedObject, e);
      }
    }
    private static T GetParent<T>(DependencyObject element) where T : DependencyObject
    {
        do
        {
          element = VisualTreeHelper.GetParent(element);
        }
        while ((element != null) && !(element is T));
        return (element as T);
    }
    private static void Scroll(ScrollBar scroll, Point pt)
    {
      VisualTreeHelper.HitTest(scroll, null, delegate(HitTestResult result)
      {
        DependencyObject element = result.VisualHit;
        RepeatButton parent = element as RepeatButton;
        if (parent == null)
        {
          parent = GetParent<RepeatButton>(element);
        }
        if ((parent != null) && (parent.Command != null))
        {
          (parent.Command as RoutedCommand).Execute(null, scroll);
        }
        return HitTestResultBehavior.Stop;
      }, new PointHitTestParameters(pt));
    }
    private void EndDragDrop(DragEventArgs e)
    {
      SetInsertionIndex(base.AssociatedObject, -1);
      SetDraggingOverIndex(base.AssociatedObject, -1);
      SetMousePosition(base.AssociatedObject, new Point());
      SetIsDragging(base.AssociatedObject, false);
    }
    private void StartDragDrop(DragEventArgs e)
    {
      SetIsDragging(base.AssociatedObject, true);
      this.UpdateState(e);
    }
    private bool UpdateInsertionIndex(ItemsControl itemsControl, Point pt, int itemIndex)
    {
      if (itemIndex == -1)
      {
        SetInsertionIndex(itemsControl, itemsControl.Items.Count);
        return true;
      }
      FrameworkElement relativeTo = itemsControl.ItemContainerGenerator.ContainerFromIndex(itemIndex) as FrameworkElement;
      pt = itemsControl.TranslatePoint(pt, relativeTo);
      double num = this.InsertionWidth / 2.0;
      if (GetItemOrientation(itemsControl) == Orientation.Horizontal)
      {
        if (pt.X < num)
        {
          SetInsertionIndex(itemsControl, itemIndex);
          return true;
        }
        if ((relativeTo.ActualWidth - pt.X) < num)
        {
          SetInsertionIndex(itemsControl, itemIndex + 1);
          return true;
        }
      }
      else
      {
        if (pt.Y < num)
        {
          SetInsertionIndex(itemsControl, itemIndex);
          return true;
        }
        if ((relativeTo.ActualHeight - pt.Y) < num)
        {
          SetInsertionIndex(itemsControl, itemIndex + 1);
          return true;
        }
      }
      return false;
    }
    private void UpdateState(DragEventArgs e)
    {
      DependencyObject dropObj;
      int itemIndex;
      Point position = e.GetPosition(base.AssociatedObject);
      SetMousePosition(base.AssociatedObject, position);
      ItemsControl itemsControl = base.AssociatedObject as ItemsControl;
      if (itemsControl != null)
      {
        dropObj = null;
        itemIndex = -1;
        VisualTreeHelper.HitTest(base.AssociatedObject, null, delegate(HitTestResult result)
        {
          dropObj = result.VisualHit;
          do
          {
            itemIndex = itemsControl.ItemContainerGenerator.IndexFromContainer(dropObj);
            if (itemIndex != -1)
            {
              break;
            }
            dropObj = VisualTreeHelper.GetParent(dropObj);
          }
          while ((dropObj != this.AssociatedObject) && !(dropObj is ScrollBar));
          return HitTestResultBehavior.Stop;
        }, new PointHitTestParameters(position));
        if (dropObj is ScrollBar)
        {
          ScrollBar scroll = dropObj as ScrollBar;
          Scroll(scroll, base.AssociatedObject.TranslatePoint(position, scroll));
        }
        else if (this.UpdateInsertionIndex(itemsControl, position, itemIndex))
        {
          SetDraggingOverIndex(base.AssociatedObject, -1);
        }
        else
        {
          SetInsertionIndex(base.AssociatedObject, -1);
          SetDraggingOverIndex(base.AssociatedObject, itemIndex);
        }
      }
    }

    public static IDragDropData GetDragDropData(DependencyObject obj)
    {
      return (IDragDropData)obj.GetValue(DragDropDataProperty);
    }
    public static int GetDraggingOverIndex(DependencyObject obj)
    {
      return (int)obj.GetValue(DraggingOverIndexProperty);
    }
    public static int GetInsertionIndex(DependencyObject obj)
    {
      return (int)obj.GetValue(InsertionIndexProperty);
    }
    public static bool GetIsDragging(DependencyObject obj)
    {
      return (bool)obj.GetValue(IsDraggingProperty);
    }
    public static Orientation GetItemOrientation(DependencyObject obj)
    {
      return (Orientation)obj.GetValue(ItemOrientationProperty);
    }
    public static Point GetMousePosition(DependencyObject obj)
    {
      return (Point)obj.GetValue(MousePositionProperty);
    }
    public static void SetDragDropData(DependencyObject obj, IDragDropData value)
    {
      obj.SetValue(DragDropDataProperty, value);
    }
    public static void SetItemOrientation(DependencyObject obj, Orientation value)
    {
      obj.SetValue(ItemOrientationProperty, value);
    }
    private static void SetDraggingOverIndex(DependencyObject obj, int value)
    {
      obj.SetValue(DraggingOverIndexPropertyKey, value);
    }
    private static void SetInsertionIndex(DependencyObject obj, int value)
    {
      obj.SetValue(InsertionIndexPropertyKey, value);
    }
    private static void SetIsDragging(DependencyObject obj, bool value)
    {
      obj.SetValue(IsDraggingPropertyKey, value);
    }
    private static void SetMousePosition(DependencyObject obj, Point value)
    {
      obj.SetValue(MousePositionPropertyKey, value);
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      base.AssociatedObject.PreviewMouseMove += new MouseEventHandler(this.AssociatedObject_MouseMove);
      base.AssociatedObject.PreviewGiveFeedback += new GiveFeedbackEventHandler(this.AssociatedObject_GiveFeedback);
      base.AssociatedObject.PreviewQueryContinueDrag += new QueryContinueDragEventHandler(this.AssociatedObject_QueryContinueDrag);
      base.AssociatedObject.DragEnter += new DragEventHandler(this.AssociatedObject_DragEnter);
      base.AssociatedObject.DragLeave += new DragEventHandler(this.AssociatedObject_DragLeave);
      base.AssociatedObject.Drop += new DragEventHandler(this.AssociatedObject_Drop);
      base.AssociatedObject.DragOver += new DragEventHandler(this.AssociatedObject_DragOver);
    }
    protected override void OnDetaching()
    {
      base.OnDetaching();
      base.AssociatedObject.PreviewMouseMove -= new MouseEventHandler(this.AssociatedObject_MouseMove);
      base.AssociatedObject.PreviewGiveFeedback -= new GiveFeedbackEventHandler(this.AssociatedObject_GiveFeedback);
      base.AssociatedObject.PreviewQueryContinueDrag -= new QueryContinueDragEventHandler(this.AssociatedObject_QueryContinueDrag);
      base.AssociatedObject.DragEnter -= new DragEventHandler(this.AssociatedObject_DragEnter);
      base.AssociatedObject.DragLeave -= new DragEventHandler(this.AssociatedObject_DragLeave);
      base.AssociatedObject.Drop -= new DragEventHandler(this.AssociatedObject_Drop);
      base.AssociatedObject.DragOver -= new DragEventHandler(this.AssociatedObject_DragOver);
    }


    // Properties
    public double InsertionWidth
    {
      get
      {
        return (double)base.GetValue(InsertionWidthProperty);
      }
      set
      {
        base.SetValue(InsertionWidthProperty, value);
      }
    }
    public IDragDropSource SourceHandler
    {
      get
      {
        return (IDragDropSource)base.GetValue(SourceHandlerProperty);
      }
      set
      {
        base.SetValue(SourceHandlerProperty, value);
      }
    }
    public IDragDropTarget TargetHandler
    {
      get
      {
        return (IDragDropTarget)base.GetValue(TargetHandlerProperty);
      }
      set
      {
        base.SetValue(TargetHandlerProperty, value);
      }
    }
  }
}
