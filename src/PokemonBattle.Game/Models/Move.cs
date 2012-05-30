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

    internal Move(int id, int moveType, IGameSettings settings)
    {
      Id = id;
      Type = DataService.GetMoveType(moveType);
      PP = new PairValue((int)(Type.PP * settings.PPUp));
    }
    internal Move(int id, Move move, IGameSettings settings)
    {
      Id = id;
      Type = move.Type;
      PP = new PairValue(5);
    }
  }
}
