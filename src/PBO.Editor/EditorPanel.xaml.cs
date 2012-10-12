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
using LightStudio.PokemonBattle.Data;

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
      DataContext = EditorVM.Current;
    }

    public bool Window_Closing()
    {
      bool cancel = false;
      try
      {
        var pm = EditorVM.Current.EditingPokemon;
        if (pm != null)
        {
          MessageBoxResult r = pm.ChangedConfirm();
          if (r == MessageBoxResult.Cancel) cancel = true;
          else if (r == MessageBoxResult.Yes) pm.Save();
        }
        if (!cancel) DataService.UserData.Save();
      }
      catch { }
      return cancel;
    }
  }
}
