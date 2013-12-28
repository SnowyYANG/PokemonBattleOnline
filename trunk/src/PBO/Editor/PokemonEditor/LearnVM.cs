using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Editor
{
  internal class LearnVM : ObservableObject
  {
    private static readonly PropertyChangedEventArgs ISSELECTED = new PropertyChangedEventArgs("IsSelected");

    private readonly PokemonEditorVM pokemon;

    public LearnVM(PokemonEditorVM pm, int move)
    {
      pokemon = pm;
      Move = RomData.GetMove(move);
      _isSelected = pokemon.Model.HasMove(move);
      _methods = new List<LearnMethod>();
    }
    
    public MoveType Move
    { get; private set; }

    private readonly List<LearnMethod> _methods;
    public IEnumerable<LearnMethod> Methods
    { get { return _methods; } }

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

    internal void AddMethod(LearnCategory method)
    {
      _methods.Add(new LearnMethod(method));
    }
  }
}
