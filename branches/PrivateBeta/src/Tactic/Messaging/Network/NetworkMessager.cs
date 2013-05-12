using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace LightStudio.Tactic.Messaging.Primitive
{
  internal abstract class NetworkMessager : Messager, INetworkMessager
  {
    private Dispatcher sendDispatcher;
    private object receivingLock;
    public bool Receiving { get; private set; }
    public abstract IPAddress RemoteAddress { get; }

    protected NetworkMessager()
    {
      this.receivingLock = new object();
      this.sendDispatcher = new Dispatcher("Messager", true);
    }

    public override void StartReceive()
    {
      if (Receiving || IdDisposed) return;
      lock (receivingLock)
      {
        if (Receiving) return;
        Receiving = true;
      }
      StartReceiveInternal();
    }

    /// <summary>
    /// the internal implementation of StartReceive method, 
    /// which is only called once (when Receiving is false)
    /// </summary>
    protected abstract void StartReceiveInternal();

    public override void Send(IMessage message)
    {
      if (IdDisposed) return;
      sendDispatcher.BeginInvoke(
          new Action(() => SendInternal(message)));
    }

    /// <summary>
    /// the internal implementation of SendInternal, 
    /// all the calls to this method are arranged and invoked on the same thread
    /// </summary>
    protected abstract void SendInternal(IMessage message);

    protected override void DisposeManagedResources()
    {
      sendDispatcher.Dispose();
      base.DisposeManagedResources();
    }
  }
}
