using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;
using PokemonBattleOnline.PBO.Editor;

namespace PokemonBattleOnline.PBO
{
  /// <summary>
  /// Interaction logic for EditorView.xaml
  /// </summary>
  public partial class EditorPanel : UserControl
  {  
    public EditorPanel()
    {
      InitializeComponent();
      gridbg.Fill = SBrushes.GetGridTileBrush(16, SBrushes.NewBrush(0x66000000));//opacity=0.4
    }

    public void Init()
    {
      DataContext = EditorVM.Current;
    }
    public void SetGridBg(double x, double y)
    {
      gridbg.Margin = new Thickness(x, y, 0, 0);
    }

    public bool Window_Closing()
    {
      bool cancel = false;
      try
      {
        var pm = EditorVM.Current.EditingPokemon;
        if (pm != null && pm.ChangedConfirm() != MessageBoxResult.Cancel) EditorVM.Current.Save();
      }
      catch { }
      return cancel;
    }
  }
}
