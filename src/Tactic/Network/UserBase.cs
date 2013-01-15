using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Tactic.Network
{
  public abstract class UserBase : IPackReceivedListener
  {
    protected internal readonly INetworkUser Network;

    internal UserBase(INetworkUser network)
    {
      Network = network;
      network.Listener = this;
    }

    public IPEndPoint EndPoint
    { get { return Network.EndPoint; } }
    internal DateTime LastPack
    { get; private set; }

    protected abstract void OnPackReceived(byte[] pack);
    void IPackReceivedListener.OnPackReceived(byte[] pack)
    {
      OnPackReceived(pack);
      LastPack = DateTime.Now;
    }
  }
}
