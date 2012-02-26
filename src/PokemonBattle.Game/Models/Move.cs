using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class Move
  {
    public readonly int Id;
    public MoveType Type { get; private set; }
    public PairValue PP { get; private set; }
    //public Pokemon Owner { get; private set; }

    /// <summary>
    /// CanSelect不代表技能一定能用，http://www.smogon.com/dp/articles/move_restrictions#disable
    /// </summary>
    public bool CanSelect { get; internal set; }

    public Move(int moveType, GameSettings settings)
    {
      Id = settings.NextId();
      Type = DataService.GetMoveType(moveType);
      PP = new PairValue((int)(Type.PP * settings.PPUp));
    }
    public Move(int id, Move move, GameSettings settings)
    {
      Id = id;
      Type = move.Type;
      PP = new PairValue(5);
    }
  }
}
