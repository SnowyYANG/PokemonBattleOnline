using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PokemonBattleOnline.Tactic.Network
{
  public abstract class ClientBase : IPackReceivedListener
  {
    public Action BadPack = delegate { };
    protected readonly INetworkClient Network;
    
    protected ClientBase(INetworkClient network)
    {
      Network = network;
      network.Listener = this;
      network.Disconnect += OnDisconnect;
    }

    protected virtual void OnDisconnect()
    {
    }

    protected void OnBadPack()
    {
      Network.Dispose();
      BadPack();
    }
    public void Send(byte[] pack)
    {
      Network.Send(pack);
    }

    protected abstract void OnPackReceived(byte[] pack);
    void IPackReceivedListener.OnPackReceived(byte[] pack)
    {
      OnPackReceived(pack);
    }
  }
}
