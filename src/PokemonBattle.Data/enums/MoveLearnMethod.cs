using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
  public enum MoveLearnMethod : byte
  {
    Invalid,
    Lv,
    Machine,
    Egg,
    Tutor,
    PreEvolution,
    XD, //净化
    Gift //礼物
  }
}
