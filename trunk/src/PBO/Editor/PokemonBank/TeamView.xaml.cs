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

namespace PokemonBattleOnline.PBO.Editor
{
  /// <summary>
  /// Interaction logic for Team.xaml
  /// </summary>
  public partial class TeamView : UserControl
  {
    public TeamView()
    {
      InitializeComponent();
    }

    private TeamVM ViewModel;
  
    protected override void OnDragEnter(DragEventArgs e)
    {
      base.OnDragEnter(e);
    }
    protected override void OnDragLeave(DragEventArgs e)
    {
      base.OnDragLeave(e);
    }
    protected override void OnDragOver(DragEventArgs e)
    {
      base.OnDragOver(e);
    }
    protected override void OnDrop(DragEventArgs e)
    {
      base.OnDrop(e);
    }

    private void NameBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      BeginEdit();
    }
    private void NameEditor_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter) EndEdit();
    }
    private void NameEditor_LostFocus(object sender, RoutedEventArgs e)
    {
      EndEdit();
    }

    private void BeginEdit()
    {
      NameEditor.Text = ((TeamVM)DataContext).Name;
      NameBlock.Visibility = Visibility.Collapsed;
      NameEditor.Visibility = Visibility.Visible;
      NameEditor.Focus();
    }
    private void EndEdit()
    {
      ((TeamVM)DataContext).Name = NameEditor.Text;
      NameEditor.Visibility = System.Windows.Visibility.Collapsed;
      NameBlock.Visibility = System.Windows.Visibility.Visible;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      ViewModel = DataContext as TeamVM;
      if (TeamVM.New == ViewModel && ViewModel != null) BeginEdit();
    }
  }
}
