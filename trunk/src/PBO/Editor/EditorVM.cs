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

    private EditorVM(IEnumerable<PokemonTeam> teams)
    {
      _teams = new ObservableCollection<TeamVM>(teams.Select((t) => new TeamVM(t)));
      _teams.Insert(0, null);
      _battleTeams = new ObservableCollection<TeamVM>(_teams.Where((t) => t != null && t.CanBattle));
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

    private readonly ObservableCollection<TeamVM> _battleTeams;
    public ObservableCollection<TeamVM> BattleTeams
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

    public void Save()
    {
      UserData.Save(Teams.Where((t) => t != null).Select((t) => t.Model));
    }
  }
}
