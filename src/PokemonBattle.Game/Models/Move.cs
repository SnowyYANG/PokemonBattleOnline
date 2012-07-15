using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class Move
  {
    public MoveType Type { get; private set; }
    public PairValue PP { get; private set; }

    internal Move(int moveType, IGameSettings settings)
    {
      Type = DataService.GetMove(moveType);
      PP = new PairValue((int)(Type.PP * settings.PPUp));
    }
    internal Move(Move move, IGameSettings settings)
    {
      Type = move.Type;
      PP = new PairValue(5);
    }
  }
}
