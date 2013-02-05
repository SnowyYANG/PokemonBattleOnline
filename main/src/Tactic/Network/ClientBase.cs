using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PokemonBattleOnline.Tactic.Network
{
  public abstract class ClientBase : IDisposable, IPackReceivedListener
  {
    private const int KEEP_ALIVE = 30000;
    
    public Action BadPack = delegate { };
    protected readonly INetworkClient Network;
    private readonly Timer KeepAlive;

    protected ClientBase(INetworkClient network)
    {
      Network = network;
      network.Listener = this;
      network.Disconnect += OnDisconnect;
      KeepAlive = new Timer(OnKeepAlive, null, KEEP_ALIVE, KEEP_ALIVE);
    }

    private void OnKeepAlive(object state)
    {
      Network.SendEmpty();
    }
    
    protected virtual void OnDisconnect()
    {
      Dispose();
    }
    protected void OnBadPack()
    {
      BadPack();
      Dispose();
    }

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
