using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Messaging
{
  public enum UserState
  {
    Normal,
    Afk,
    Battling,
    Spectating,
    Aggressive
  }
  public class UE
  {
    public short RoomId
    { get; internal set; }
    public byte Avatar
    { get; set; }
    public UserState State
    { get; set; }
  }
}
