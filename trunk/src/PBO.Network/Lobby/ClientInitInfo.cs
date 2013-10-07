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
    [DataMember]
    public readonly int User;

    public ClientInitInfo(int user)
    {
      User = user;
    }

    private readonly string[] _names;
    public IEnumerable<string> Names
    { get { return _names; } }
    private readonly int[] Avatars;
  }
}
