using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PokemonBattleOnline.Network
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public enum Seat
  {
    [DataMember(Name = "player00")]
    Player00,
    [DataMember(Name = "player01")]
    Player01,
    [DataMember(Name = "player10")]
    Player10,
    [DataMember(Name = "player11")]
    Player11,
    [DataMember(Name = "spectator")]
    Spectator
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
