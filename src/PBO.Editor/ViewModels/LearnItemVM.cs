using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;
using LightStudio.Tactic.Globalization;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class LearnItemVM : ObservableObject
  {
    private static readonly PropertyChangedEventArgs ISSELECTED = new PropertyChangedEventArgs("IsSelected");

    private readonly PokemonEditorVM pokemon;

    public LearnItemVM(PokemonEditorVM pm, MoveType move)
    {
      pokemon = pm;
      Move = move;
      _isSelected = pokemon.Model.MoveIds.Contains(move.Id);
    }
    
    public MoveType Move
    { get; private set; }

    public string MoveName
    { get { return Move.GetLocalizedName(); } }

    private bool _isSelected;
    public bool IsSelected
    {
      get
      {
        return _isSelected;
      }
      set
      {
        if (_isSelected != value)
        {
          if (value) _isSelected = pokemon.AddMove(Move);
          else
          {
            pokemon.RemoveMove(Move);
            _isSelected = false;
          }
          OnPropertyChanged(ISSELECTED);
        }
      }
    }
  }
}
