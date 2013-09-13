using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class ClientInitInfo
  {
    [DataMember(EmitDefaultValue = false)]
    public readonly string Welcome;
    [DataMember]
    public readonly int User;

    public ClientInitInfo(Server server, int user)
    {
      Welcome = server.Welcome;
      User = user;
    }

    private readonly string[] _names;
    public IEnumerable<string> Names
    { get { return _names; } }
    private readonly int[] Avatars;
  }
}
