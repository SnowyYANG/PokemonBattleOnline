using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Editor
{
  internal class EditorVM : ObservableObject
  {
    public static readonly EditorVM Current;

    static EditorVM()
    {
      Current = new EditorVM(UserData.Current);
    }

    private EditorVM(UserData model)
    {
      _model = model;
    }

    private readonly UserData _model;
    public UserData Model
    { get { return _model; } }
    
    private PokemonEditorVM _editingPokemon;
    public PokemonEditorVM EditingPokemon
    { 
      get { return _editingPokemon; }
      set
      {
        _editingPokemon = value;
        OnPropertyChanged("EditingPokemon");
      }
    }
    
    public void EditPokemon(PokemonVM pm)
    {
      if (EditingPokemon != null && EditingPokemon.Origin != pm)
      {
        MessageBoxResult r = EditingPokemon.ChangedConfirm();
        if (r == MessageBoxResult.Yes) EditingPokemon.Save();
        else if (r == MessageBoxResult.Cancel) return;
      }
      EditingPokemon = new PokemonEditorVM(pm);
    }
    public void EndEditing()
    {
      if (EditingPokemon != null)
      {
        MessageBoxResult r = EditingPokemon.ChangedConfirm();
        if (r == MessageBoxResult.Yes) EditingPokemon.Save();
        else if (r == MessageBoxResult.Cancel) return;
        EditingPokemon = null;
      }
    }
  }
}
