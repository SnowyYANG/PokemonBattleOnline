using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Tactic.Network
{
  public interface IPackReceivedListener
  {
    void OnPackReceived(byte[] pack);
  }
  public interface INetworkIO : IDisposable
  {
    IPackReceivedListener Listener { get; set; }
    void Send(byte[] pack);
  }
  public interface INetworkUser : INetworkIO
  {
    event Action Disconnect;
    int Id { get; }
    IPEndPoint EndPoint { get; }
  }
  public interface INetworkClient : INetworkIO
  {
    event Action Disconnect;
    IPEndPoint Server { get; }
  }
}
