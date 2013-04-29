using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Data
{
  public enum LearnCategory : byte
  {
    Lv,
    Machine,
    Egg,
    Tutor,
    Other //净化，礼物，剧情
  }
  public class LearnMethod
  {
    public LearnCategory Method
    { get; private set; }
    public int Gen
    { get; private set; }
    public int Detail
    { get; private set; }
  }
}
