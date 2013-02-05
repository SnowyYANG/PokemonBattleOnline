using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Tactic.Globalization;
using PokemonBattleOnline.PBO.UIElements;

namespace PokemonBattleOnline.PBO.Editor
{
  internal class LearnVM : ObservableObject
  {
    private static readonly PropertyChangedEventArgs ISSELECTED = new PropertyChangedEventArgs("IsSelected");

    private readonly PokemonEditorVM pokemon;

    public LearnVM(PokemonEditorVM pm, MoveType move)
    {
      pokemon = pm;
      Move = move;
      _isSelected = pokemon.Model.HasMove(move.Id);
    }
    
    public MoveType Move
    { get; private set; }

    public IEnumerable<LearnMethod> Methods
    { get; private set; }

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
