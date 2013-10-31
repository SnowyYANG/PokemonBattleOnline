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
  /// Interaction logic for PokemonIcon.xaml
  /// </summary>
  public partial class PokemonIcon : UserControl
  {
    public PokemonIcon()
    {
      InitializeComponent();
    }

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

    bool click;
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);
      click = true;
    }
    protected override void OnMouseLeave(MouseEventArgs e)
    {
      base.OnMouseLeave(e);
      click = false;
    }
    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonUp(e);
      if (click)
      {
        var pm = (PokemonVM)DataContext;
        EditorVM.Current.EditPokemon(pm);
      }
    }
  }
}
