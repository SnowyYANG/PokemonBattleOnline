using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game.Sp;

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
      int aS = 1, bS = 1;

      if (a.Action == PokemonAction.WillSwitch && b.Action == PokemonAction.WillSwitch) goto SPEED;
      if (a.Action == PokemonAction.WillSwitch) return -1;
      if (b.Action == PokemonAction.WillSwitch) return 1;

      {
        int aP = a.SelectedMove.Priority;
        int bP = b.SelectedMove.Priority;
        if (a.SelectedMove.Type.Category == Data.MoveCategory.Status && a.Ability.Prankster()) aP++;
        if (a.SelectedMove.Type.Category == Data.MoveCategory.Status && b.Ability.Prankster()) bP++;
        if (aP != bP)
          return bP - aP;
      }

      {
        int aItem = a.Item.CompareValue(a);
        int bItem = b.Item.CompareValue(b);
        if (aItem != bItem)//1=先制爪/先制果发动 0=无道具 -1=后攻尾/满腹香炉发动<--同时带这个慢的先出
          return (bItem - aItem);
        if (aItem == -1) aS = bS = -1;
      }

      {
        bool aIsStall = a.Ability.Stall();
        bool bIsStall = b.Ability.Stall();
        if (aIsStall == bIsStall) goto SPEED;
        if (aIsStall) return 1;
        if (bIsStall) return -1;
      }

    SPEED:
      {
        var aField = a.Controller.Board[a.Pokemon.TeamId];
        int sRaw = a.OnboardPokemon.Static.Speed;
        if (aField.HasCondition("TailWind")) sRaw <<= 1;
        if (aField.HasCondition("Swamp")) sRaw = (aS + 1) >> 2; //小数点是0.5以下就舍去，如果是0.75就四舍五入
        aS *= OnboardPokemon.Get5D(sRaw, a.OnboardPokemon.Lv5D.Speed);
      }
      {
        var bField = b.Controller.Board[b.Pokemon.TeamId];
        int sRaw = b.OnboardPokemon.Static.Speed;
        if (bField.HasCondition("TailWind")) sRaw <<= 1;
        if (bField.HasCondition("Swamp")) sRaw = (sRaw + 1) >> 2;
        bS *= OnboardPokemon.Get5D(sRaw, b.OnboardPokemon.Lv5D.Speed);
      }
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
