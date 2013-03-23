using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Tactic.Network
{
  public abstract class UserBase : IPackReceivedListener, IDisposable
  {
    public readonly INetworkUser Network;

    protected UserBase(INetworkUser network)
    {
      Network = network;
      network.Listener = this;
    }

    public IPEndPoint EndPoint
    { get { return Network.EndPoint; } }

    protected abstract void OnPackReceived(byte[] pack);
    void IPackReceivedListener.OnPackReceived(byte[] pack)
    {
      OnPackReceived(pack);
    }

    public virtual void Dispose()
    {
      Network.Dispose();
    }
  }
}
