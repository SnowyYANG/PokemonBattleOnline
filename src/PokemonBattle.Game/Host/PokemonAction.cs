using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public enum PokemonAction
  {
    Debuting,
    WaitingForInput,
    WillSwitch,
    WillMove,
    SwitchPrepared,
    MovePrepared,
    Switching,
    Moving,
    Done,
    Stiff,
    Faint
  }
}
