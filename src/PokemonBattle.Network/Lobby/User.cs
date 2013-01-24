using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network
{
  public enum UserState
  {
    Normal,
    Afk,
    Battling,
    Spectating,
    Aggressive
  }
  public class User
  {
    internal User(int id, string name, ushort avatar)
    {
      _id = id;
      _name = name;
      Avatar = avatar;
    }

    private readonly int _id;
    public int Id
    { get { return _id; } }
    private readonly string _name;
    public string Name
    { get { return _name; } }
    public short RoomId
    { get; internal set; }
    public ushort Avatar
    { get; set; }
    public UserState State
    { get; set; }
  }
}
