using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network
{
  internal enum RemoveUserReason
  {
    Admin,
    BadPack,
    Disconnect
  }
}