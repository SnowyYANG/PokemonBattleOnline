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
using System.Reflection;
using System.Windows.Media.Animation;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Editor
{
  /// <summary>
  /// Interaction logic for PokemoEditorView.xaml
  /// </summary>
  public partial class PokemonEditorView : UserControl
  {
    VirtualizingStackPanel panel;

    public PokemonEditorView()
    {
      InitializeComponent();
      grid.Fill = SBrushes.GetHorizontalTileBrush(25, SBrushes.NewBrush(0x80ffffff));
      Natures.ItemsSource = Enum.GetValues(typeof(PokemonNature));
    }

    private PokemonEditorVM VM
    { get { return (PokemonEditorVM)DataContext; } }

    private void SelectedMove_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (((ContentPresenter)sender).Content != null)
      {
        var move = ((LearnedMove)((ContentPresenter)sender).Content).Move.Id;
        for (int i = 0; i < learnsetlist.Items.Count; ++i)
          if (((LearnVM)learnsetlist.Items[i]).Move.Id == move)
          {
            learnsetlist.SelectedIndex = i;
            if (panel == null) panel = typeof(ItemsControl).InvokeMember("_itemsHost", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField, null, learnsetlist, null) as VirtualizingStackPanel;
            if (panel != null) panel.SetVerticalOffset(panel.ScrollOwner.ScrollableHeight * learnsetlist.SelectedIndex / learnsetlist.Items.Count);
            break;
          }
      }
    }

    private void ResetEv_Click(object sender, RoutedEventArgs e)
    {
      VM.Model.Ev.SetStat(StatType.All, 0);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
      VM.Save();
      EditorVM.Current.Save();
    }
    private void Reset_Click(object sender, RoutedEventArgs e)
    {
      VM.ResetToLastSaved();
    }
    private void Close_Click(object sender, RoutedEventArgs e)
    {
      if (EditorVM.Current.EditingPokemon != null && EditorVM.Current.EditingPokemon.Close()) EditorVM.Current.EditingPokemon = null;
    }
    private void Happiness_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      VM.Model.Happiness = VM.Model.Happiness == 255 ? 0 : 255;
    }

    private void QuickText_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        var text = QuickText.Text.Trim();
        if (!string.IsNullOrWhiteSpace(text))
        {
          var pn = text.ToInt();
          if (1 <= pn && pn <= RomData.Pokemons.Count()) VM.PokemonSpecies = RomData.GetPokemon(pn);
          else
          {
            var m = GameString.Move(text);
            if (m != null) VM.AddMove(m);
            else
            {
              var i = GameString.Item(text);
              if (i != 0) VM.HeldItem = i;
              else
              {
                var a = GameString.Ability(text);
                if (a != 0) VM.Model.Ability = a;
                else
                {
                  var n = GameString.Nature(text);
                  if (n.HasValue) VM.Model.Nature = n.Value;
                  else
                  {
                    var p = GameString.PokemonSpecies(text);
                    if (p != null) VM.PokemonSpecies = p;
                    else return;
                  }
                }
              }
            }
          }
        } //if (!string.Is
        QuickText.Clear();
      }
    }

    private void Pokemon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      ((PokemonVM)((ContentControl)sender).Content).Edit();
    }
  }
}
