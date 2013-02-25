using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network.Lobby
{
  internal static class PackHeaders
  {
    public const byte CS_COMMAND = 0;
    public const byte P2P_COMMAND = 1;

    public const byte PUBLIC_CHAT = 10;
    public const byte ROOM_CHAT = 11;
  }
}
