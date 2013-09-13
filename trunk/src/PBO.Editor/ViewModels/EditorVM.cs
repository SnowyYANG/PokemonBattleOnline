using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using PokemonBattleOnline.Tactic.Globalization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Editor
{
  internal class EditorVM : ObservableObject
  {
    public static readonly EditorVM Current;

    static EditorVM()
    {
      Current = new EditorVM(DataService.UserData);
    }

    private EditorVM(UserData model)
    {
      Boxes = new GroupVM(model.Boxes, false);
      Teams = new GroupVM(model.Teams, true);
      Recycler = new RecyclerVM(model.Recycler);
      OpenWindows = new ObservableCollection<object>();
      PokemonBT.PokemonDeleted += (pm) =>
        {
          if (EditingPokemon != null && EditingPokemon.Origin == pm) EndEditing();
        };
    }

    private PokemonEditorVM _editingPokemon;
    public PokemonEditorVM EditingPokemon
    { get { return _editingPokemon; } }
    public Visibility EditingPokemonVisibility
    { get { return EditingPokemon == null ? Visibility.Collapsed : Visibility.Visible; } }
    
    public GroupVM Boxes { get; private set; }
    public GroupVM Teams { get; private set; }
    public RecyclerVM Recycler { get; private set; }
    
    public ObservableCollection<object> OpenWindows { get; private set; }

    public void Switch(CollectionVM folder)
    {
      folder.IsOpen = !folder.IsOpen;
      if (folder.IsOpen) OpenWindows.Add(folder);
      else OpenWindows.Remove(folder);
    }

    public void EditPokemon(PokemonData pm)
    {
      if (EditingPokemon != null && EditingPokemon.Origin != pm)
      {
        MessageBoxResult r = EditingPokemon.ChangedConfirm();
        if (r == MessageBoxResult.Yes) EditingPokemon.Save();
        else if (r == MessageBoxResult.Cancel) return;
      }
      _editingPokemon = new PokemonEditorVM(pm);
      OnPropertyChanged("EditingPokemon");
      OnPropertyChanged("EditingPokemonVisibility");
    }
    public void EndEditing()
    {
      if (EditingPokemon != null)
      {
        MessageBoxResult r = EditingPokemon.ChangedConfirm();
        if (r == MessageBoxResult.Yes) EditingPokemon.Save();
        else if (r == MessageBoxResult.Cancel) return;
        _editingPokemon = null;
        OnPropertyChanged("EditingPokemon");
        OnPropertyChanged("EditingPokemonVisibility");
      }
    }
  }
}
