using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Messaging.Room
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
