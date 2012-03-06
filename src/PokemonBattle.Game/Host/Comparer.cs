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
      if (a.Action == PokemonAction.WillWithdraw && b.Action == PokemonAction.WillWithdraw) return CompareSpeed(a.Speed, b.Speed);
      if (a.Action == PokemonAction.WillWithdraw) return 1;
      if (b.Action == PokemonAction.WillWithdraw) return -1;

      if (a.SelectedMove.Priority != b.SelectedMove.Priority)
        return a.SelectedMove.Priority - b.SelectedMove.Priority;

#warning unfinished Items
      //if (a.Item != b.Item)//1=先制爪/先制果发动 0=无道具 -1=后攻尾/满腹香炉发动
      //  return (a.Item - b.Item);

      bool aIsStall = a.HasWorkingAbility(AbilityIds.STALL);
      bool bIsStall = b.HasWorkingAbility(AbilityIds.STALL);
      if (aIsStall && !bIsStall) return -1;
      if (!aIsStall && bIsStall) return 1;
      return CompareSpeed(a.Speed, b.Speed);
    }
    public int Compare(Tile a, Tile b)
    {
      return CompareSpeed(a.Speed, b.Speed);
    }
    private int CompareSpeed(int a, int b)
    {
      if (board.HasCondition("TrickRoom")) return b - a;
      else return a - b;
    }
  }
}
