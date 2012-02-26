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
            if (data == null)
                return;

            DragDropInfo dragDropInfo = CollectionDragDropTarget.GetDragDropInfo(sender, e, data);
            data.Actions = dragDropInfo.Action;

            if (data.Actions == DragDropActions.MoveTo || data.Actions == DragDropActions.CopyTo)
            {
                IFolderViewModel folder = dragDropInfo.TargetFolder;
                PokemonViewModel pm = data.Pokemon;
                if (data.Actions == DragDropActions.CopyTo)
                {
                    pm = pm.Clone();
                }
                else
                {
                    data.SourceFolder.RemovePokemon(pm);
                }
                folder.InsertPokemon(folder.Pokemons.Count, pm);
            }
        }

        public void HandleDragOver(UIElement sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(IDragDropData)) as PokemonDragDropData;
            if (data == null)
                return;
            DragDropState.SetDragDropData(sender, data);
            data.Actions = CollectionDragDropTarget.GetDragDropInfo(sender, e, data).Action;
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
            int folderIndex = DragDropState.GetDraggingOverIndex(target);

            var collection = (target as FrameworkElement).DataContext as CollectionViewModel;
            if (collection != null && folderIndex != -1)
            {
                dragDropInfo.TargetFolder = collection.Folders[folderIndex];
                if (dragDropInfo.TargetFolder.CanAddPokemon)
                {
                    if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                    {
                        dragDropInfo.Action = DragDropActions.CopyTo;
                    }
                    else if (dragDropInfo.TargetFolder != data.SourceFolder)
                    {
                        dragDropInfo.Action = DragDropActions.MoveTo;
                    }
                }
            }
            return dragDropInfo;
        }

        private class DragDropInfo
        {
            public DragDropActions Action
            { get; set; }

            public IFolderViewModel TargetFolder
            { get; set; }
        }

    }
}
