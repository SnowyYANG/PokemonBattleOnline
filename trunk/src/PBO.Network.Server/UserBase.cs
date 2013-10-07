using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Network
{
  internal abstract class UserBase : IPackReceivedListener, IDisposable
  {
    internal readonly TcpUser Network;

    protected UserBase(TcpUser network)
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
