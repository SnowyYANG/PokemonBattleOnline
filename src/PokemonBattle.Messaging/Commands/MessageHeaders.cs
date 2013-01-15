using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Messaging
{
  internal static class MessageHeaders
  {
    public const byte CHAT = 1;
    public const byte CHALLENGE = 2;
    public const byte ACCEPT_CHALLENGE = 3;
    public const byte REFUSE_CHALLENGE = 4;
    public const byte CANCEL_CHALLENGE = 5;
    
    public const byte REGISTER_ROOM = 21;
    
    public const byte GAME_H2C = 11;
    public const byte GAME_C2H = 12;
  }
}
