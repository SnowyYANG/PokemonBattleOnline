using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
  [Flags]
  public enum InputType
  {
    Struggle = 1,
    UseMove = 2,
    Sendout = 4,
  }
}
