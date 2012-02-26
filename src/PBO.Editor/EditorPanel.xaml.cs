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
using System.Windows.Media.Animation;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  /// <summary>
  /// Interaction logic for EditorView.xaml
  /// </summary>
  public partial class EditorPanel : UserControl
  {  
    public EditorPanel()
    {
      InitializeComponent();
      gridbg.Margin = new Thickness(0 - PBO.Helper.Random.Next(16), 0 - PBO.Helper.Random.Next(16), 0, 0);
      gridbg.Fill = PBO.UIElements.Brushes.GetGridTileBrush(16, PBO.Helper.NewBrush(0x66000000));//opacity=0.4
    }

    public void Init()
    {
      DataContext = Editor.CurrentEditor;
      EditorConfig.Init(); //总觉得不对头..
    }

    public bool Window_Closing()
    {
      bool cancel = false;
      try
      {
        if (Editor.CurrentEditor.EditingPokemon != null)
        {
          var p = Editor.CurrentEditor.EditingPokemon;
          MessageBoxResult r = p.ChangedConfirm();
          if (r == MessageBoxResult.Cancel) cancel = true;
          else if (r == MessageBoxResult.Yes) p.Save();
        }
        if (!cancel) Editor.CurrentEditor.Model.Save();
      }
      catch { }
      return cancel;
    }
  }
}
