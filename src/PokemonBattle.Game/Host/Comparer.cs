using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  internal sealed class Comparer : IComparer<PokemonProxy>, IComparer<Tile>
  {
    private readonly Board board;

    public Comparer(Board board)
    {
      this.board = board;
    }

    public int Compare(PokemonProxy a, PokemonProxy b)
    {
      if (a.Action == PokemonAction.WillSwitch && b.Action == PokemonAction.WillSwitch) goto SPEED;
      if (a.Action == PokemonAction.WillSwitch) return -1;
      if (b.Action == PokemonAction.WillSwitch) return 1;

      if (a.SelectedMove.Priority != b.SelectedMove.Priority)
        return b.SelectedMove.Priority - a.SelectedMove.Priority;

#warning unfinished Items
      //if (a.Item != b.Item)//1=先制爪/先制果发动 0=无道具 -1=后攻尾/满腹香炉发动
      //  return (b.Item - a.Item);

      bool aIsStall = a.Ability.Id == AbilityIds.STALL;
      bool bIsStall = b.Ability.Id == AbilityIds.STALL;
      if (aIsStall == bIsStall) goto SPEED;
      if (aIsStall) return 1;
      if (bIsStall) return -1;

      SPEED:
      return CompareSpeed(a.Speed, b.Speed);
    }
    public int Compare(Tile a, Tile b)
    {
      return CompareSpeed(a.Speed, b.Speed);
    }
    private int CompareSpeed(int a, int b)
    {
      if (board.HasCondition("TrickRoom")) return a - b;
      else return b - a;
    }
  }
}
