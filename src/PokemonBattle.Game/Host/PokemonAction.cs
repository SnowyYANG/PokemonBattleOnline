using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public enum PokemonAction
  {
    WaitingForInput,
    WillWithdraw,
    WillMove,
    Switching,
    Moving,
    Done,
    Stiff,
    Faint
  }
}
