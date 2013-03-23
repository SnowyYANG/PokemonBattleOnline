using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network
{
  public enum UserState
  {
    Normal,
    Battling,
    Afk,
    Quited
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  public class User
  {
    internal User(int id, string name, ushort avatar)
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

    private UserState _state;
    public UserState State
    {
      get { return _state; }
      internal set { if (_state != UserState.Quited) _state = value; }
    }
    [DataMember(Name = "d", EmitDefaultValue = false)]
    private UserState s
    {
      get { return State == UserState.Battling ? default(UserState) : State; }
      set { State = value; }
    }

    public int RoomId
    { get; internal set; }
    [DataMember(EmitDefaultValue = false, Order = 0)]
    private int r
    {
      get { return State == UserState.Battling ? RoomId : 0; }
      set
      {
        if (value != 0)
        {
          State = UserState.Battling;
          RoomId = value;
        }
      }
    }
  }
}
