using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class User : ObservableObject
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

    [DataMember(Name = "d")]
    internal int RoomId;
    
    private Room _room;
    public Room Room
    {
      get { return _room; }
      internal set
      {
        if (_room != value)
        {
          _room = value;
          RoomId = _room.Id;
        }
      }
    }

    public Seat Seat
    { get; internal set; }
  }
}
