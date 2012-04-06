using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  internal static class SpMoves
  {
    public const int SNORE = 173;
    public const int SLEEP_TALK = 214;
    public const int FOCUS_PUNCH = 264;

    public static bool AvailableEvenSleeping(this MoveProxy move)
    {
      return move.Move.Type.Id == SLEEP_TALK || move.Move.Type.Id == SNORE;
    }
    public static void PreMove(this MoveProxy move)
    {
      if (move.Move.Type.Id == FOCUS_PUNCH)
      {
      }
    }
  }
}
