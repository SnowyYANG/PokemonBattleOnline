﻿using System;
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

    private EditorVM(IEnumerable<PokemonTeam> teams)
    {
      _teams = new ObservableCollection<TeamVM>(teams.Select((t) => new TeamVM(t)));
      _teams.Insert(0, null);
      _battleTeams = new ObservableCollection<PokemonTeam>(teams.Where((t) => t.CanBattle));
    }

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

    private readonly ObservableCollection<TeamVM> _teams;
    public ObservableCollection<TeamVM> Teams
    { get { return _teams; } }

    private readonly ObservableCollection<PokemonTeam> _battleTeams;
    public ObservableCollection<PokemonTeam> BattleTeams
    { get { return _battleTeams; } }

    public void NewTeam()
    {
      var t = new TeamVM(new PokemonTeam());
      TeamVM.New = t;
      _teams.Insert(1, t);
    }
    public void ImportTeam(string text)
    {
      var t = new TeamVM(UserData.ImportTeam(text));
      TeamVM.New = t;
      _teams.Insert(1, t);
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

    public void Save()
    {
      UserData.Save(Teams.Where((t) => t != null).Select((t) => t.Model));
    }
  }
}
