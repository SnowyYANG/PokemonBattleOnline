using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LightStudio.PokemonBattle.PBO.UIElements.Interactivity;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    public class FolderDragDropSource : IDragDropSource
    {

        public void HandleMouseMove(UIElement sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;
            var selector = sender as Selector;
            if (selector == null || selector.SelectedItem == null)
                return;
            //find the visual item hit
            var itemContainer = selector.ItemContainerGenerator.
                ContainerFromIndex(selector.SelectedIndex) as FrameworkElement;
            if (!FolderDragDropSource.HitElement(e.GetPosition(itemContainer), itemContainer))
                return;

            var selectedItem = selector.SelectedItem as PokemonViewModel;

            IFolderViewModel folder = selectedItem.Folder;
            int selectedIndex = selector.SelectedIndex;
            var data =
                new PokemonDragDropData(selectedItem, DragDropActions.All, folder, selectedIndex);

            var dataObj = new DataObject(typeof(IDragDropData), data);
            DragDrop.DoDragDrop(selector, dataObj, DragDropEffects.All);
        }

        private static bool HitElement(Point pt, FrameworkElement element)
        {
            return pt.X >= 0 && pt.Y >= 0 && pt.X <= element.ActualWidth && pt.Y <= element.ActualHeight;
        }

        public void HandleQueryContinueDrag(UIElement sender, QueryContinueDragEventArgs e)
        {
            if (e.EscapePressed)
                e.Action = DragAction.Cancel;
        }

        public void HandleGiveFeedback(UIElement sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            e.Handled = true;
        }
    }
}
