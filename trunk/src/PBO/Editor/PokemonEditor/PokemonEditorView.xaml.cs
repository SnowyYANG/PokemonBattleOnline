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
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Editor
{
  /// <summary>
  /// Interaction logic for PokemoEditorView.xaml
  /// </summary>
  public partial class PokemonEditorView : UserControl
  {
    static readonly Brush NATUREE;
    static readonly Brush NATURED;
    static PokemonEditorView()
    {
      NATUREE = SBrushes.NewBrush(0xffffcccc);
      NATURED = SBrushes.NewBrush(0xff99ccff);
    }
    
    VirtualizingStackPanel panel;

    public PokemonEditorView()
    {
      InitializeComponent();
      Natures.ItemsSource = Enum.GetValues(typeof(PokemonNature));
      Natures.SelectionChanged += Natures_SelectionChanged;
    }

    void Natures_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (VM.Model != null)
      {
        var n = VM.Model.Nature;
        var r = n.StatRevise(StatType.Atk);
        LabelA.Foreground = r == 10 ? Brushes.White : r > 10 ? NATUREE : NATURED;
        r = n.StatRevise(StatType.Def);
        LabelD.Foreground = r == 10 ? Brushes.White : r > 10 ? NATUREE : NATURED;
        r = n.StatRevise(StatType.SpAtk);
        LabelSA.Foreground = r == 10 ? Brushes.White : r > 10 ? NATUREE : NATURED;
        r = n.StatRevise(StatType.SpDef);
        LabelSD.Foreground = r == 10 ? Brushes.White : r > 10 ? NATUREE : NATURED;
        r = n.StatRevise(StatType.Speed);
        LabelS.Foreground = r == 10 ? Brushes.White : r > 10 ? NATUREE : NATURED;
      }
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
            if (m != null)
            {
              foreach (var l in VM.Learnset)
                if (l.Move == m) l.IsSelected = true;
            }
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

    private void Gender_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      VM.Gender = VM.Gender == PokemonGender.Male ? PokemonGender.Female : PokemonGender.Male;
    }
  }
}
