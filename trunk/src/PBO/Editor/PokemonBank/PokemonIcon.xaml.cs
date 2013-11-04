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

    PokemonVM _vm;
    PokemonVM VM
    { 
      get
      {
        if (_vm == null) _vm = (PokemonVM)DataContext;
        return _vm;
      }
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
    protected override void OnMouseEnter(MouseEventArgs e)
    {
      base.OnMouseEnter(e);
      Stroke.StrokeThickness = 3;
    }
    protected override void OnMouseLeave(MouseEventArgs e)
    {
      base.OnMouseLeave(e);
      click = false;
      Stroke.ClearValue(Polygon.StrokeThicknessProperty);
    }
    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonUp(e);
      if (click) VM.Edit();
    }

    private void Remove_Click(object sender, RoutedEventArgs e)
    {
      var message = VM.IsEditing ? "正在编辑该精灵，放弃编辑并删除？" : "删除精灵？";
      if (MessageBox.Show(message, "PBO", MessageBoxButton.YesNo) == MessageBoxResult.Yes) VM.Model = null;
    }
    private void Paste_Click(object sender, RoutedEventArgs e)
    {
      if (VM.Model == null || MessageBox.Show(VM.IsEditing ? "正在编辑的精灵，放弃编辑并覆盖？" : "覆盖原有精灵？", "PBO", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
      {
        var pm = Game.UserData.ImportPokemon(Clipboard.GetText());
        if (pm == null) MessageBox.Show("不是合法的精灵。");
        else
        {
          VM.Model = pm;
          if (VM.IsEditing) EditorVM.Current.EditingPokemon = new PokemonEditorVM(VM);
        }
      }
    }
    private void Copy_Click(object sender, RoutedEventArgs e)
    {
      if (VM.Model != null) Clipboard.SetText(Game.UserData.Export(VM.Model));
    }
    private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      var model = VM.Model;
      Copy.IsEnabled = Remove.IsEnabled = model != null;
      Paste.IsEnabled = Clipboard.ContainsText();
    }
  }
}
