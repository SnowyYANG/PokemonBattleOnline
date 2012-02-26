using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Logging;

namespace LightStudio.Tactic.Messaging.Lobby
{
  public abstract class ClientBase : DisposableObject
  {
    protected IMessageClient MessageClient
    { get; private set; }

    protected Dispatcher Dispatcher
    { get; private set; }

    protected ClientBase(IMessageClient client)
    {
      this.Dispatcher = new Dispatcher(true);
      SetMessageClient(client);
    }

    private void SetMessageClient(IMessageClient client)
    {
      MessageClient = client;
      client.Received += (sender, e) => OnReceiveInternal(e.Message);
      client.Connected += (sender, e) => OnConnected();
      client.ConnectFailed += (sender, e) => OnConnectFailed(e.Exception);
      client.Disconnected += (sender, e) => OnDisconnected();
    }
    private void OnReceiveInternal(IMessage message)
    {
      if (message == null)
      {
        LoggerFacade.LogWarn("ClientBase.OnReceive : Null message");
        return;
      }
      Dispatcher.BeginInvoke((Action<IMessage>)OnReceive, message);
    }

    protected void Connect()
    {
      MessageClient.Connect();
    }
    protected void Disconnect()
    {
      MessageClient.Disconnect();
    }
    protected abstract void OnReceive(IMessage message);
    protected void StartReceive()
    {
      MessageClient.StartReceive();
    }
    protected void Send(IMessage message)
    {
      MessageClient.Send(message);
    }

    protected override void DisposeManagedResources()
    {
      base.DisposeManagedResources();
      MessageClient.Dispose();
      Dispatcher.Dispose();
    }

    #region Events

    public event EventHandler Connected = delegate { };
    protected virtual void OnConnected()
    {
      Connected(this, EventArgs.Empty);
    }

    public event EventHandler Disconnected = delegate { };
    protected virtual void OnDisconnected()
    {
      Disconnected(this, EventArgs.Empty);
    }

    public event EventHandler<MessageExceptionEventArgs> ConnectFailed = delegate { };
    protected virtual void OnConnectFailed(MessageException exception)
    {
      ConnectFailed(this, new MessageExceptionEventArgs(exception));
    }

    #endregion
  }
}
