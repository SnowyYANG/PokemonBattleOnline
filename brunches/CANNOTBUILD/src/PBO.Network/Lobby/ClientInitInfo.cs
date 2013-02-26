using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class ClientInitInfo
  {
    public readonly string Welcome;
    public readonly int[] Ids;
    public readonly string[] Names;
    public readonly int[] Avatars;
    public readonly UserState[] States;

    public ClientInitInfo(Server server)
    {
      Welcome = server.Welcome;
      //var n = server.Users.Count();
    }
  }
}
