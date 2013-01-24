using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network
{
  [DataContract(Namespace = Namespaces.JSON)]
  public class ClientInitInfo
  {
    string Description;
  }
}
