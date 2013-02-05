using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network.Room
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
