using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PokemonDataEditor.ViewModel
{
    public class WorkspaceViewModel : DependencyObject
    {
        #region AttachProperty

        private static void DbClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Control control = obj as Control;
            if (control != null)
            {
                if ((bool)e.NewValue)
                    control.MouseDoubleClick += OpenItem;
                else
                    control.MouseDoubleClick -= OpenItem;
            }
        }

        public static bool GetDbClickToOpen(Control obj)
        {
            return (bool)obj.GetValue(DbClickToOpenProperty);
        }

        public static void SetDbClickToOpen(Control obj, bool value)
        {
            obj.SetValue(DbClickToOpenProperty, value);
        }

        public static readonly DependencyProperty DbClickToOpenProperty =
            DependencyProperty.RegisterAttached("DbClickToOpen", typeof(bool), typeof(WorkspaceViewModel),
            new UIPropertyMetadata(false, DbClickChanged));


        private static Dictionary<Button, bool> clickToClose = new Dictionary<Button, bool>();

        public static bool GetClickToClose(Button btn)
        {
            if (clickToClose.ContainsKey(btn))
                return clickToClose[btn];
            else
                return false;
        }

        public static void SetClickToClose(Button btn, bool value)
        {
            if (GetClickToClose(btn) != value)
            {
                if (value)
                    btn.Click += CloseItem;
                else
                    btn.Click -= CloseItem;
            }
        }

        #endregion

        public static ObservableCollection<object> OpenItems
        { get; private set; }

        static WorkspaceViewModel()
        {
            OpenItems = new ObservableCollection<object>();

        }

        public static void OpenItem(object sender, RoutedEventArgs e)
        {
            OpenItems.Add((sender as ListBoxItem).DataContext);
            
        }

        public static void CloseItem(object sender, RoutedEventArgs e)
        {
            OpenItems.Remove((sender as Button).DataContext);
        }
    }
}
