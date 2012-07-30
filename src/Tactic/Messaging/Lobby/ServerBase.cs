using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Logging;
using LightStudio.Tactic.Messaging.Primitive;

namespace LightStudio.Tactic.Messaging.Lobby
{
  public abstract class ServerBase : DisposableObject
  {
    public bool IsStarted { get; private set; }
    protected IMessageServer MessageServer { get; private set; }
    protected Dispatcher Dispatcher { get; private set; }

    public ServerBase(IMessageServer server)
    {
      this.Dispatcher = new Dispatcher("Server", true);
      this.MessageServer = server;
      this.MessageServer.Received += (sender, e) => OnReceiveInternal(e.MessagerId, e.Message);
      this.MessageServer.SessionEnded += (sender, e) => OnClientExited(e.SessionId);
    }

    public virtual void Start()
    {
      LoggerFacade.LogDebug("ServerBase start");
      MessageServer.Start();
      IsStarted = true;
    }

    public virtual void Stop()
    {
      LoggerFacade.LogDebug("ServerBase stop");
      MessageServer.Stop();
      IsStarted = false;
    }

    protected void Send(int clientId, IMessage message)
    {
      MessageServer.Send(clientId, message);
    }

    protected void Send(IEnumerable<int> clientIds, IMessage message)
    {
      foreach (var clientId in clientIds)
        MessageServer.Send(clientId, message);
    }

    protected void Broadcast(IMessage message)
    {
      MessageServer.Broadcast(message);
    }

    protected override void DisposeManagedResources()
    {
      base.DisposeManagedResources();
      MessageServer.Dispose();
      Dispatcher.Dispose();
    }

    private void OnReceiveInternal(int clientId, IMessage message)
    {
      if (message == null)
      {
        LoggerFacade.LogWarn("ServerBase.OnReceive : null message");
        return;
      }
      Dispatcher.BeginInvoke(new Action<int, IMessage>(OnReceive), clientId, message);
    }

    protected abstract void OnReceive(int clientId, IMessage message);

    //TODO: thread safe?
    protected abstract void OnClientExited(int clientId);
  }
}
