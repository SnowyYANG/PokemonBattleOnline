﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Tactic.Network
{
  public abstract class ClientBase : IPackReceivedListener
  {
    public Action BadPack = delegate { };
    protected INetworkClient Network;
    
    protected ClientBase(INetworkClient network)
    {
      Network = network;
      network.Listener = this;
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
