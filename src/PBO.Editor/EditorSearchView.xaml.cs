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

namespace LightStudio.PokemonBattle.PBO.Editor
{
  /// <summary>
  /// Interaction logic for EditorSearchView.xaml
  /// </summary>
  public partial class EditorSearchView : UserControl
  {
    public EditorSearchView()
    {
      InitializeComponent();
    }

    private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
      canvas.Visibility = System.Windows.Visibility.Collapsed;
    }

    private void canvas_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (canvas.IsVisible) Background = Brushes.Transparent;
      else Background = null;
    }

    private void UserControl_MouseEnter(object sender, MouseEventArgs e)
    {
      canvas.Visibility = System.Windows.Visibility.Visible;
    }
  }
}
