using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  public enum RoomState
  {
    Invalid,
    RoomOpen,
    GameStarted,
    GameEnd,
    RoomClosed
  }
}
