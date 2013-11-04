using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Editor
{
  class PokemonVM : ObservableObject
  {
    public PokemonVM(TeamVM container, int index)
    {
      _container = container;
      _index = index;
      //Commands = new ObservableCollection<MenuCommand>();
      //if (Model.Container != UserData.Current.Recycler)
      //{
      //  EditCommand = new MenuCommand("Edit", () => EditorVM.Current.EditPokemon(Model));
      //  Commands.Add(EditCommand);
      //}
      //RemoveCommand = new MenuCommand("Remove", () => Model.Container.Remove(Model));
      //Commands.Add(RemoveCommand);
    }

    private TeamVM _container;
    public TeamVM Container
    { get { return _container; } }

    private int _index;
    public int Index
    { get { return _index; } }

    private bool _isEditing;
    public bool IsEditing
    {
      get { return _isEditing; }
      set
      {
        if (_isEditing != value)
        {
          _isEditing = value;
          OnPropertyChanged("IsEditing");
        }
      }
    }
    
    public PokemonData Model
    {
      get { return _container.Model.Pokemons[_index]; }
      set
      {
        if (value == null)//editing?
        {
          if (_index != 0 && EditorVM.Current.EditingPokemon != null && EditorVM.Current.EditingPokemon.Origin == this) EditorVM.Current.EditingPokemon.Origin = _container[_index - 1];
          if (_container.Model.Pokemons.ValueOrDefault(_index + 1) == null)
          {
            _container.Model.Pokemons[_index] = null;
            if (_index < 5) _container[_index + 1].OnPropertyChanged("Icon");
            OnPropertyChanged();
          }
          else
          {
            _container.Model.Pokemons[_index] = _container[_index + 1].Model;
            _container[_index + 1].Model = null;
          }
        }
        else if (_index == 0 || _container.Model.Pokemons[_index - 1] != null)
        {
          _container.Model.Pokemons[_index] = value;
          if (_index < 5) _container[_index + 1].OnPropertyChanged("Icon");
          OnPropertyChanged();
        }
      }
    }

    public Effect Effect
    {
      get
      {
        if (Model == null) return null;
        var ev = Model.Ev;
        return
          Model.Form.Species.Number != 132 && Model.Moves.Count() != 4 ?
          R.MagentaShadow :
          508 > ev.Sum() ?
          R.OrangeShadow :
          null;
      }
    }

    public ImageSource Icon
    { get { return Model == null ? Index == 0 || Container[Index - 1].Model != null ? R.P00000 : null : ImageService.GetPokemonIcon(Model.Form, Model.Gender); } }

    public void Edit()
    {
      if (EditorVM.Current.EditingPokemon == null || EditorVM.Current.EditingPokemon.Origin != this && EditorVM.Current.EditingPokemon.Close()) EditorVM.Current.EditingPokemon = new PokemonEditorVM(this);
    }
    //public MenuCommand EditCommand
    //{ get; private set; }
    //public MenuCommand RemoveCommand
    //{ get; private set; }
    //public ObservableCollection<MenuCommand> Commands
    //{ get; private set; }
  }
}
