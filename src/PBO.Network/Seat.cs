using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PokemonBattleOnline.Network
{
  public enum Seat
  {
    [EnumMember(Value = "player00")]
    Player00 = 0,
    [EnumMember(Value = "player01")]
    Player01 = 1,
    [EnumMember(Value = "player10")]
    Player10 = 2,
    [EnumMember(Value = "player11")]
    Player11 = 3,
    [EnumMember(Value = "spectator")]
    Spectator = 4
  }
  public static class SeatExtensions
  {
    public static int TeamId(this Seat seat)
    {
      return (int)seat >> 1;
    }
    public static int TeamIndex(this Seat seat)
    {
      return (int)seat % 2;
    }
  }
}
