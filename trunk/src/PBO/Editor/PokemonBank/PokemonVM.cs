﻿using System;
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
    }

    private TeamVM _container;
    public TeamVM Container
    { get { return _container; } }

    public PokemonVM Actual
    { 
      get
      {
        var pms = _container.Model.Pokemons;
        for (int i = _index; i > 0; --i)
          if (pms[i - 1] != null) return _container[i];
        return this;
      }
    }

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
    
    private bool _isDragging;
    public bool IsDragging
    {
      get { return _isDragging; }
      set
      {
        if (_isDragging != value)
        {
          _isDragging = value;
          OnPropertyChanged("IsDragging");
        }
      }
    }
    private int _dropState;
    public int DropState
    {
      get { return _dropState; }
      set
      {
        if (_dropState != value)
        {
          _dropState = value;
          OnPropertyChanged("DropState");
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
          if (_index < 5 && EditorVM.Current.EditingPokemon != null && EditorVM.Current.EditingPokemon.Origin == _container[_index + 1]) EditorVM.Current.EditingPokemon.Origin = this;
          if (_container.Model.Pokemons.ValueOrDefault(_index + 1) == null)
          {
            _container.Model.Pokemons[_index] = null;
            if (_index < 5) _container[_index + 1].OnPropertyChanged("Icon");
            OnPropertyChanged();
          }
          else
          {
            _container.Model.Pokemons[_index] = _container.Model.Pokemons[_index + 1];
            _container[_index + 1].Model = null;
          }
        }
        else
        {
          var i = Actual.Index;
          _container.Model.Pokemons[i] = value;
          if (i < 5) _container[i + 1].OnPropertyChanged("Icon");
          Actual.OnPropertyChanged();
        }
      }
    }

    public ImageSource Icon
    { get { return Model == null ? Index == 0 || Container[Index - 1].Model != null ? R.P00000 : null : ImageService.GetPokemonIcon(Model.Form, Model.Gender); } }

    public Brush Background
    {
      get
      {
        if (Model == null) return Brushes.Transparent;
        var ev = Model.Ev;
        return Model.Form.Species.Number != 132 && Model.Moves.Count() != 4 ? SBrushes.MagentaM : 508 > ev.Sum() ? SBrushes.OrangeM : Brushes.Transparent;
      }
    }

    public void Edit()
    {
      if (EditorVM.Current.EditingPokemon == null || EditorVM.Current.EditingPokemon.Origin != Actual && EditorVM.Current.EditingPokemon.Close()) EditorVM.Current.EditingPokemon = new PokemonEditorVM(Actual);
    }
  }
}
