using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Tactic.Network
{
  [DataContract(Namespace = Namespaces.JSON)]
  public class ClientInitInfo<TLobby, TUser>
  {
  }
}
