using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network
{
  public class Room
  {
    public int Id;
    public int[,] Players;
    public HashSet<int> Spectators;
    public bool ReadyA1;
    public bool ReadyA2;
    public bool ReadyB1;
    public bool ReadyB2;
    public bool Battling;
  }
}
