using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
  public class MessageClient : Messager, IMessageClient
  {
    #region Events

    public event EventHandler Connected = delegate { };
    protected void OnConnected()
    {
      messager = connector.Messager;
      messager.Received += (sender, e) => OnReceive(e.Message);
      messager.UnhandledException += (sender, e) => OnUnhandledException(e.Exception);
      IsConnected = true;

      Connected(this, EventArgs.Empty);
    }

    public event EventHandler Disconnected = delegate { };
    protected void OnDisconnected()
    {
      IsConnected = false;
      messager = null;

      Disconnected(this, EventArgs.Empty);
    }

    public event EventHandler<MessageExceptionEventArgs> ConnectFailed = delegate { };
    protected virtual void OnConnectFailed(MessageException exception)
    {
      ConnectFailed(this, new MessageExceptionEventArgs(exception));
    }

    #endregion

    private readonly IConnector connector;
    private IMessager messager;
    
    internal MessageClient(IConnector connector)
    {
      Contract.Requires(connector != null);

      this.connector = connector;
      this.connector.Connected += (sender, e) => OnConnected();
      this.connector.ConnectFailed += (sender, e) => OnConnectFailed(e.Exception);
      this.connector.Disconnected += (sender, e) => OnDisconnected();
    }

    public bool IsConnected
    { get; private set; }

    public void Connect()
    {
      connector.Connect();
    }

    public void Disconnect()
    {
      connector.Disconnect();
    }

    public override void Send(IMessage message)
    {
      var deliverer = this.messager;//for thread-safe
      if (deliverer != null)
        deliverer.Send(message);
    }

    public override void StartReceive()
    {
      var messager = this.messager;//for thread-safe
      if (messager != null)
        messager.StartReceive();
    }

    protected override void OnUnhandledException(MessageException exception)
    {
      if (IdDisposed)
        return;

      base.OnUnhandledException(exception);
      Disconnect();
    }

    protected override void DisposeManagedResources()
    {
      connector.Dispose();
      if (messager != null)
      {
        messager.Dispose();
        messager = null;
      }

      base.DisposeManagedResources();
    }
  }
}
