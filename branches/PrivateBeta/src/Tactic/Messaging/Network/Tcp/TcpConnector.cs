using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics.Contracts;
using LightStudio.Tactic.Logging;

namespace LightStudio.Tactic.Messaging.Primitive
{
  internal class TcpConnector : Connector, IConnector
  {
    private static readonly LingerOption LS3;
    static TcpConnector()
    {
      LS3 = new LingerOption(true, 3);
    }
    #region Properties

    protected TcpClient TcpClient
    { get; private set; }

    protected IInterpreter Interpreter
    { get; private set; }

    public IPAddress HostAddress
    { get; private set; }

    public int Port
    { get; private set; }

    public override bool IsConnected
    {
      get
      {
        return TcpClient != null && TcpClient.Connected;
      }
    }

    private Lazy<TcpMessager> messager;
    public override IMessager Messager
    {
      get
      {
        if (IsConnected)
        {
          return messager.Value;
        }
        return null;
      }
    }

    #endregion

    #region Events

    protected override void OnConnected()
    {
      LoggerFacade.LogDebug(string.Format("TcpConnector : Connected to {0}", HostAddress));
      base.OnConnected();
    }

    protected override void OnConnectFailed(MessageException exception)
    {
      LoggerFacade.LogDebug(
          string.Format("TcpConnector : Failed to connect to {0}\r\n\tDetails : {1}",
          HostAddress, exception.InnerMessage));
      base.OnConnectFailed(exception);
    }

    protected override void OnDisconnected()
    {
      LoggerFacade.LogDebug(string.Format("TcpConnector : Disconnected from {0}", HostAddress));
      base.OnDisconnected();
    }

    #endregion

    public TcpConnector(IPAddress hostAddress, int port, IInterpreter interpreter)
    {
      Contract.Requires(hostAddress != null);
      Contract.Requires(interpreter != null);

      this.HostAddress = hostAddress;
      this.Port = port;
      this.Interpreter = interpreter;
    }

    public override void Connect()
    {
      if (IdDisposed)
        return;

      if (!IsConnected)
      {
        TcpClient = new TcpClient() { LingerState = LS3 };
        ResetMessager();
        try
        {
          LoggerFacade.LogDebug(string.Format("TcpConnector : Connecting to {0}",
              HostAddress));

          TcpClient.BeginConnect(HostAddress, Port, ConnectCallback, null);
        }
        catch (Exception ex)
        {
          OnSafeConnectFailed("Exception occurred in TcpConnector.Connect", ex);
        }
      }
    }

    private void ConnectCallback(IAsyncResult asyncResult)
    {
      try
      {
        TcpClient.EndConnect(asyncResult);
        OnSafeConencted();
      }
      catch (Exception ex)
      {
        OnSafeConnectFailed("Exception occurred in TcpConnector.ConnectCallback", ex);
      }
    }

    public override void Disconnect()
    {
      if (IdDisposed)
        return;

      TcpClient.Close();
      //if (IsConnected) //it should be HasConnected?
      //{
      OnSafeDisconnected();
      //}
    }

    protected void ResetMessager()
    {
      messager = new Lazy<TcpMessager>(() => new TcpMessager(TcpClient, Interpreter));
    }

    protected override void DisposeManagedResources()
    {
      TcpClient.Close();
    }
  }
}
