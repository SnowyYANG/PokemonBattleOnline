using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace PokemonBattleOnline.Tactic.Network
{
  public interface INetworkClient : IDisposable
  {
    IPEndPoint Server { get; }
    IPackReceivedListener Listener { get; set; }
    void Send(byte[] pack);
  }
}
