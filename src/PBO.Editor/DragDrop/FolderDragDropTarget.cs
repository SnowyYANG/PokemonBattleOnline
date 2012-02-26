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
    public class FolderDragDropTarget : IDragDropTarget
    {

        public void HandleDrop(UIElement sender, DragEventArgs e)
        {
            DragDropState.SetDragDropData(sender, null);
            e.Effects = DragDropEffects.None;

            var data = e.Data.GetData(typeof(IDragDropData)) as PokemonDragDropData;
            if (data == null)
                return;

            DragDropInfo dragDropInfo = FolderDragDropTarget.GetDragDropInfo(sender, e, data);
            data.Actions = dragDropInfo.Action;
            switch (data.Actions)
            {
                case DragDropActions.MoveTo:
                    FolderDragDropTarget.MoveItemTo(dragDropInfo.Folder, dragDropInfo.InsertIndex, data);
                    break;
                case DragDropActions.CopyTo:
                    FolderDragDropTarget.CopyItemTo(dragDropInfo.Folder, dragDropInfo.InsertIndex, data);
                    break;
                case DragDropActions.SwapWith:
                    FolderDragDropTarget.SwapItem(dragDropInfo.Folder, dragDropInfo.PokemonIndex, data);
                    break;
                case DragDropActions.Replace:
                    FolderDragDropTarget.SubstitueItem(dragDropInfo.Folder, dragDropInfo.PokemonIndex, 
                        data);
                    break;
            }
        }

        #region Actions

        private static void MoveItemTo(IFolderViewModel folder, int destIndex, 
            PokemonDragDropData data)
        {
            //remove the origin item to prevent exceeding the size
            data.SourceFolder.RemovePokemon(data.Pokemon);

            //adjust index if it is movement within a folder
            if (folder == data.SourceFolder && destIndex > data.PokemonIndexInFolder)
                destIndex--;

            folder.InsertPokemon(destIndex, data.Pokemon);
        }

        private static void CopyItemTo(IFolderViewModel folder, int destIndex, 
            PokemonDragDropData data)
        {
            folder.InsertPokemon(destIndex, data.Pokemon.Clone());
        }

        private static void SubstitueItem(IFolderViewModel folder, int destIndex, 
            PokemonDragDropData data)
        {
            folder.Pokemons[destIndex].RemoveSelf();
            folder.InsertPokemon(destIndex, data.Pokemon.Clone());
        }

        private static void SwapItem(IFolderViewModel folder, int swapIndex, 
            PokemonDragDropData data)
        {
            PokemonViewModel pokemon = data.Pokemon;
            int originalIndex = data.PokemonIndexInFolder;
            PokemonViewModel swapPokemon = folder.Pokemons[swapIndex];

            folder.RemovePokemon(swapPokemon);
            data.SourceFolder.RemovePokemon(pokemon);
            if (data.SourceFolder == folder && swapIndex < originalIndex)//swap within a folder
            {
                folder.InsertPokemon(swapIndex, pokemon);
                folder.InsertPokemon(originalIndex, swapPokemon);
                /*
                if (swapIndex < originalIndex)
                {
                }
                else
                {
                    folder.InsertPokemon(originalIndex, swapPokemon);
                    folder.InsertPokemon(swapIndex, pokemon);
                }
                 */
            }
            else//swap between folders
            {
                data.SourceFolder.InsertPokemon(originalIndex, swapPokemon);
                folder.InsertPokemon(swapIndex, pokemon);
            }
        }

        #endregion

        public void HandleDragOver(UIElement sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(IDragDropData)) as PokemonDragDropData;
            if (data == null)
                return;
            DragDropState.SetDragDropData(sender, data);
            data.Actions = FolderDragDropTarget.GetDragDropInfo(sender, e, data).Action;
        }

        public void HandleDragLeave(UIElement sender, DragEventArgs e)
        {
            DragDropState.SetDragDropData(sender, null);
        }

        public void HandleDragEnter(UIElement sender, DragEventArgs e)
        {
            DragDropState.SetDragDropData(sender,
                e.Data.GetData(typeof(IDragDropData)) as IDragDropData);
        }

        private static DragDropInfo GetDragDropInfo(UIElement target, DragEventArgs e,
            PokemonDragDropData data)
        {
            var dragDropInfo = new DragDropInfo();
            dragDropInfo.Action = DragDropActions.None;

            dragDropInfo.Folder = (target as FrameworkElement).DataContext as IFolderViewModel;
            if (dragDropInfo.Folder == null)
                return dragDropInfo;

            dragDropInfo.InsertIndex = DragDropState.GetInsertionIndex(target);
            dragDropInfo.PokemonIndex = DragDropState.GetDraggingOverIndex(target);

            if (dragDropInfo.PokemonIndex != -1)
            {
                if (data.SourceFolder != dragDropInfo.Folder ||
                    data.PokemonIndexInFolder != dragDropInfo.PokemonIndex)
                {
                    if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                    {
                        dragDropInfo.Action = DragDropActions.Replace;
                    }
                    else
                    {
                        dragDropInfo.Action = DragDropActions.SwapWith;
                    }
                }
            }
            else if (dragDropInfo.InsertIndex != -1)
            {
                if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                {
                    if (dragDropInfo.Folder.CanAddPokemon)
                        dragDropInfo.Action = DragDropActions.CopyTo;
                }
                else if (data.SourceFolder != dragDropInfo.Folder)//movement between folders
                {
                    if (dragDropInfo.Folder.CanAddPokemon)
                        dragDropInfo.Action = DragDropActions.MoveTo;
                }
                else//movement within folder
                {
                    if (IsValidMovement(data.PokemonIndexInFolder, dragDropInfo.InsertIndex))
                        dragDropInfo.Action = DragDropActions.MoveTo;
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

            public IFolderViewModel Folder
            { get; set; }
        }
    }
}
