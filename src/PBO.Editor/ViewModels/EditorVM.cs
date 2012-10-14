﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using LightStudio.Tactic.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class EditorVM : ViewModelBase
  {
    public static readonly EditorVM Current;

    static EditorVM()
    {
      Current = new EditorVM();
    }

    private EditorVM()
    {
      this.Boxes = new GroupVM(DataService.UserData.Boxes, false);
      this.Teams = new GroupVM(DataService.UserData.Teams, true);
      this.Recycler = new RecyclerVM(DataService.UserData.Recycler);
      this.OpenWindows = new ObservableCollection<object>();
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
