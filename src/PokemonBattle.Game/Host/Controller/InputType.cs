using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  [Flags]
  public enum InputType
  {
    Struggle = 1,
    UseMove = 2,
    Sendout = 4,
  }
}
