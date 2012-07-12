using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Windows.Media.Animation;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  /// <summary>
  /// Interaction logic for PokemoEditorView.xaml
  /// </summary>
  public partial class PokemoEditorView : UserControl
  {
    VirtualizingStackPanel panel;

    public PokemoEditorView()
    {
      InitializeComponent();
      grid.Fill = PBO.UIElements.Brushes.GetHorizontalTileBrush(25, PBO.Helper.NewBrush(0x80ffffff));
    }

    private void SelectedMove_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (((ContentPresenter)sender).Content == null) return;
      learnsetlist.SelectedItem = ((ContentPresenter)sender).Content;
      if (panel == null)
        panel = typeof(ItemsControl).InvokeMember("_itemsHost", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField, null, learnsetlist, null) as VirtualizingStackPanel;
      if (panel != null)
        panel.SetVerticalOffset(panel.ScrollOwner.ScrollableHeight * learnsetlist.SelectedIndex / learnsetlist.Items.Count);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
      EditingPokemonViewModel vm = DataContext as EditingPokemonViewModel;
      if (vm != null) vm.Save();
    }
  }
}
