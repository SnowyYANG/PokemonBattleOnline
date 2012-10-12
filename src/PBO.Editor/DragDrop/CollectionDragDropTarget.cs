using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LightStudio.PokemonBattle.PBO.UIElements.Interactivity;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  public class CollectionDragDropTarget : IDragDropTarget
  {
    public void HandleDrop(UIElement sender, DragEventArgs e)
    {
      DragDropState.SetDragDropData(sender, null);
      e.Effects = DragDropEffects.None;

      var data = e.Data.GetData(typeof(IDragDropData)) as PokemonDragDropData;
      if (data != null)
      {
        DragDropInfo dragDropInfo = CollectionDragDropTarget.GetDragDropInfo(sender, e, data);
        data.Actions = dragDropInfo.Action;

        if (data.Actions == DragDropActions.MoveTo || data.Actions == DragDropActions.CopyTo)
        {
          PokemonCollection folder = dragDropInfo.Target;
          PokemonData pm = data.Actions == DragDropActions.CopyTo ? data.Pokemon : data.Pokemon.Clone();
          if (data.Actions == DragDropActions.MoveTo) data.Source.Remove(pm);
          folder.Add(pm);
        }
      }
    }

    public void HandleDragOver(UIElement sender, DragEventArgs e)
    {
      var data = e.Data.GetData(typeof(IDragDropData)) as PokemonDragDropData;
      if (data != null)
      {
        DragDropState.SetDragDropData(sender, data);
        data.Actions = CollectionDragDropTarget.GetDragDropInfo(sender, e, data).Action;
      }
    }

    public void HandleDragLeave(UIElement sender, DragEventArgs e)
    {
      DragDropState.SetDragDropData(sender, null);
    }

    public void HandleDragEnter(UIElement sender, DragEventArgs e)
    {
      DragDropState.SetDragDropData(sender, e.Data.GetData(typeof(IDragDropData)) as IDragDropData);
    }

    private static DragDropInfo GetDragDropInfo(UIElement target, DragEventArgs e, PokemonDragDropData data)
    {
      var dragDropInfo = new DragDropInfo();
      dragDropInfo.Action = DragDropActions.None;

      int i = DragDropState.GetDraggingOverIndex(target);
      var collection = (target as FrameworkElement).DataContext as CollectionGroup;
      if (collection != null && i != -1)
      {
        dragDropInfo.Target = collection[i];
        if (dragDropInfo.Target.CanAdd)
        {
          if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey)) dragDropInfo.Action = DragDropActions.CopyTo;
          else if (dragDropInfo.Target != data.Source) dragDropInfo.Action = DragDropActions.MoveTo;
        }
      }
      return dragDropInfo;
    }

    private class DragDropInfo
    {
      public DragDropActions Action
      { get; set; }

      public PokemonCollection Target
      { get; set; }
    }

  }
}
