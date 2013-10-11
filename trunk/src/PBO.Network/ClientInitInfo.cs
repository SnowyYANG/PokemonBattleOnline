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

    [DataMember]
    private readonly int[] _ids;
    [DataMember]
    private readonly string[] _names;
    [DataMember]
    private readonly ushort[] _avatars;

    public IEnumerable<User> Users
    { 
      get
      {
        var us = new User[_ids.Length];
        for (int i = 0; i < us.Length; ++i) us[i] = new User(_ids[i], _names[i], _avatars[i]);
        return us;
      }
    }
    public IEnumerable<Room> Rooms
    { 
      get
      {
        throw new NotImplementedException();
      }
    }
  }
}
