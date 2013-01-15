using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PokemonBattleOnline.PBO.UIElements.Interactivity;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.PBO.Editor
{
  public class FolderDragDropTarget : IDragDropTarget
  {
    public void HandleDrop(UIElement sender, DragEventArgs e)
    {
      DragDropState.SetDragDropData(sender, null);
      e.Effects = DragDropEffects.None;

      var data = e.Data.GetData(typeof(IDragDropData)) as PokemonDragDropData;
      if (data == null) return;

      DragDropInfo dragDropInfo = FolderDragDropTarget.GetDragDropInfo(sender, e, data);
      data.Actions = dragDropInfo.Action;
      switch (data.Actions)
      {
        case DragDropActions.MoveTo:
          FolderDragDropTarget.MoveItemTo(dragDropInfo.Container, dragDropInfo.InsertIndex, data);
          break;
        case DragDropActions.CopyTo:
          FolderDragDropTarget.CopyItemTo(dragDropInfo.Container, dragDropInfo.InsertIndex, data);
          break;
        case DragDropActions.SwapWith:
          FolderDragDropTarget.SwapItem(dragDropInfo.Container, dragDropInfo.PokemonIndex, data);
          break;
        case DragDropActions.Replace:
          FolderDragDropTarget.SubstitueItem(dragDropInfo.Container, dragDropInfo.PokemonIndex, data);
          break;
      }
    }

    #region Actions

    private static void MoveItemTo(PokemonCollection dest, int destIndex, PokemonDragDropData data)
    {
      if (dest == data.Source)
      {
        var i = dest.IndexOf(data.Pokemon);
        if (i < destIndex) destIndex--;
        dest.Move(i, destIndex);
      }
      else
      {
        dest.Insert(destIndex, data.Pokemon);
        data.Source.Remove(data.Pokemon);
      }
    }

    private static void CopyItemTo(PokemonCollection folder, int destIndex, PokemonDragDropData data)
    {
      folder.Insert(destIndex, data.Pokemon.Clone());
    }

    private static void SubstitueItem(PokemonCollection folder, int destIndex, PokemonDragDropData data)
    {
      folder.RemoveAt(destIndex);
      folder.Insert(destIndex, data.Pokemon.Clone());
    }

    private static void SwapItem(PokemonCollection folder, int swapIndex, PokemonDragDropData data)
    {
      PokemonData pokemon = data.Pokemon;
      int originalIndex = data.IndexInContainer;
      PokemonData swapPokemon = folder[swapIndex];

      folder[originalIndex] = swapPokemon;
      data.Source[swapIndex] = pokemon;
    }
    #endregion

    public void HandleDragOver(UIElement sender, DragEventArgs e)
    {
      var data = e.Data.GetData(typeof(IDragDropData)) as PokemonDragDropData;
      if (data != null)
      {
        DragDropState.SetDragDropData(sender, data);
        data.Actions = FolderDragDropTarget.GetDragDropInfo(sender, e, data).Action;
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

      dragDropInfo.Container = ((target as FrameworkElement).DataContext as CollectionVM).Model;
      if (dragDropInfo.Container == null) return dragDropInfo;

      dragDropInfo.InsertIndex = DragDropState.GetInsertionIndex(target);
      dragDropInfo.PokemonIndex = DragDropState.GetDraggingOverIndex(target);

      if (dragDropInfo.PokemonIndex != -1)
      {
        if (data.Source != dragDropInfo.Container || data.IndexInContainer != dragDropInfo.PokemonIndex)
        {
          if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey)) dragDropInfo.Action = DragDropActions.Replace;
          else dragDropInfo.Action = DragDropActions.SwapWith;
        }
      }
      else if (dragDropInfo.InsertIndex != -1)
      {
        if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
        {
          if (dragDropInfo.Container.CanAdd) dragDropInfo.Action = DragDropActions.CopyTo;
        }
        else if (data.Source != dragDropInfo.Container)//movement between folders
        {
          if (dragDropInfo.Container.CanAdd) dragDropInfo.Action = DragDropActions.MoveTo;
        }
        else//movement within folder
        {
          if (IsValidMovement(data.IndexInContainer, dragDropInfo.InsertIndex)) dragDropInfo.Action = DragDropActions.MoveTo;
        }
      }
      return dragDropInfo;
    }

    private static bool IsValidMovement(int originIndex, int destIndex)
    {
      return originIndex != destIndex && originIndex + 1 != destIndex;
    }

    private class DragDropInfo
    {
      public int InsertIndex
      { get; set; }

      public int PokemonIndex
      { get; set; }

      public DragDropActions Action
      { get; set; }

      public PokemonCollection Container
      { get; set; }
    }
  }
}
