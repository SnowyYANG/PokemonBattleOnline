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
      int aS = 0, bS = 0;

      if (a.Action == PokemonAction.WillSwitch && b.Action == PokemonAction.WillSwitch) goto SPEED;
      if (a.Action == PokemonAction.WillSwitch) return -1;
      if (b.Action == PokemonAction.WillSwitch) return 1;

      if (a.SelectedMove.Priority != b.SelectedMove.Priority)
        return b.SelectedMove.Priority - a.SelectedMove.Priority;

#warning unfinished Items
      //if (a.Item != b.Item)//1=先制爪/先制果发动 0=无道具 -1=后攻尾/满腹香炉发动<--同时带这个慢的先出
      //  return (b.Item - a.Item);

      bool aIsStall = a.Ability.Id == AbilityIds.STALL;
      bool bIsStall = b.Ability.Id == AbilityIds.STALL;
      if (aIsStall == bIsStall) goto SPEED;
      if (aIsStall) return 1;
      if (bIsStall) return -1;

      {
        var aField = a.Controller.Board[a.Tile.Team];
        aS = a.Speed;
        if (aField.HasCondition("TailWind")) aS <<= 1;
        if (aField.HasCondition("Swamp")) aS = (aS + 1) >> 2; //小数点是0.5以下就舍去，如果是0.75就四舍五入
      }
      {
        var bField = b.Controller.Board[b.Tile.Team];
        bS = b.Speed;
        if (bField.HasCondition("TailWind")) bS <<= 1;
        if (bField.HasCondition("Swamp")) bS = (bS + 1) >> 2;
      }

    SPEED:
      return CompareSpeed(aS, bS);
    }
    public int Compare(Tile a, Tile b)
    {
      return CompareSpeed(a.Speed, b.Speed);
    }
    private int CompareSpeed(int a, int b)
    {
      const int N = 1813; //如果一方的实际速度能力值≥N，则速度快的一方先行动。（1806＜N≤1812）
      if (a < N && b < N && board.HasCondition("TrickRoom")) return a - b;
      else return b - a;
    }
  }
}
