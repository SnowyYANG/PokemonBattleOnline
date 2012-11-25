using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
  public class LearnedMove : ObservableObject
  {
    public LearnedMove(MoveType move, int ppUp = 3)
    {
      _move = move;
      _ppUp = (byte)ppUp;
    }
    
    private MoveType _move;
    public MoveType Move
    { get { return _move; } }
    
    private byte _ppUp;
    public byte PPUp
    {
      get { return _ppUp; }
      set
      {
        if (_ppUp != value && 0 <= value && value <= 3)
        {
          _ppUp = value;
          OnPropertyChanged("PPUp");
          OnPropertyChanged("PP");
        }
      }
    }

    public int PP
    { get { return Move.PP * (5 + PPUp) / 5; } }

    public override bool Equals(object obj)
    {
      var m = obj as LearnedMove;
      return m != null && m.Move == Move && m.PPUp == PPUp;
    }
    public override string ToString()
    {
      return Move.GetLocalizedName();//纯粹用来给UI作弊的
    }
  }
  public interface IPokemonData
  {
    string Name { get; }
    PokemonForm Form { get; }
    int Lv { get; }
    PokemonGender Gender { get; }
    PokemonNature Nature { get; }
    int AbilityIndex { get; }
    int ItemId { get; }
    int Happiness { get; }
    I6D Iv { get; }
    I6D Ev { get; }
    string Chatter { get; }
    IEnumerable<LearnedMove> Moves { get; }
  }
}
