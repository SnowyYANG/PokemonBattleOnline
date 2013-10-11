using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class User
  {
    public User(int id, string name, ushort avatar)
    {
      _id = id;
      _name = name;
      _avatar = avatar;
    }

    [DataMember(Name = "a")]
    private readonly int _id;
    public int Id
    { get { return _id; } }
    [DataMember(Name = "b")]
    private readonly string _name;
    public string Name
    { get { return _name; } }
    [DataMember(Name = "c")]
    private readonly ushort _avatar;
    public ushort Avatar
    { get { return _avatar; } }

    public int RoomId
    { get; internal set; }
    public Seat Seat
    { get; internal set; }
  }
}
