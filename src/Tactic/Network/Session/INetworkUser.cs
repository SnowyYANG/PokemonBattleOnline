using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Tactic.Network
{
  public interface INetworkUser
  {
    event Action Removed;

    int Id { get; }
    IPEndPoint EndPoint { get; }
    IPackReceivedListener Listener { get; set; }

    void Send(byte[] pack);
  }
}
