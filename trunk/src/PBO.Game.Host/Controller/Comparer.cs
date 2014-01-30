﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host
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
      int aS = 1, bS = 1;

      if (a.Action == PokemonAction.WillSwitch && b.Action == PokemonAction.WillSwitch) goto SPEED;
      if (a.Action == PokemonAction.WillSwitch) return -1;
      if (b.Action == PokemonAction.WillSwitch) return 1;

      {
        var am = a.SelectedMove.Type;
        var bm = b.SelectedMove.Type;
        int aP = am.Priority;
        int bP = bm.Priority;
        if (am.Category == MoveCategory.Status && a.AbilityE(As.PRANKSTER) || am.Type == BattleType.Flying && a.AbilityE(As.GALE_WINGS)) aP++;
        if (bm.Category == MoveCategory.Status && b.AbilityE(As.PRANKSTER) || bm.Type == BattleType.Flying && b.AbilityE(As.GALE_WINGS)) bP++;
        if (aP != bP) return bP - aP;
      }

      {//1=先制爪/先制果发动 0=无道具 -1=后攻尾/满腹香炉发动<--同时带这个慢的先出 只计算不发动
        int aItem = a.ItemSpeedValue;
        int bItem = b.ItemSpeedValue;
        if (aItem != bItem) return (bItem - aItem);
        if (aItem == -1) aS = bS = -1;
      }

      {
        bool aIsStall = a.AbilityE(As.STALL);
        bool bIsStall = b.AbilityE(As.STALL);
        if (aIsStall == bIsStall) goto SPEED;
        if (aIsStall) return 1;
        if (bIsStall) return -1;
      }

    SPEED:
      aS *= a.Speed;
      bS *= b.Speed;
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
