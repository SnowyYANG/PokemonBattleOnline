using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics.Contracts;
using LightStudio.Tactic.Logging;

namespace LightStudio.Tactic.Messaging
{
  internal class TcpAcceptor : Acceptor, IAcceptor
  {

    #region Properties

    protected TcpListener Listener
    { get; private set; }

    protected IInterpreter Interpreter
    { get; private set; }

    private bool accepting;
    public override bool Accepting
    {
      get { return accepting; }
    }

    protected int Port
    { get; private set; }

    #endregion

    public TcpAcceptor(int port, IInterpreter interpreter)
    {
      Contract.Requires(interpreter != null);

      this.Port = port;
      this.Listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
      this.Interpreter = interpreter;
    }

    public override void Start()
    {
      if (IdDisposed)
        return;

      LoggerFacade.LogDebug(string.Format("TcpAcceptor : Start accepting on port {0}", Port));
      try
      {
        Listener.Start();
        accepting = true;
      }
      catch (Exception ex)
      {
        OnSafeException("Exception occurred in TcpAcceptor.Start", ex);
        throw ex;
      }
      AcceptNext();
    }

    private void AcceptNext()
    {
      try
      {
        Listener.BeginAcceptTcpClient(AcceptCallback, null);
      }
      catch (Exception ex)
      {
        OnSafeException("Exception occurred in TcpAcceptor.AcceptNext", ex);
      }
    }

    private void AcceptCallback(IAsyncResult asyncResult)
    {
      try
      {
        var client = Listener.EndAcceptTcpClient(asyncResult);
        OnSafeAccepted(new TcpMessager(client, Interpreter));
        if (Accepting)
          AcceptNext();
      }
      catch (Exception ex)
      {
        OnSafeException("Exception occurred in TcpAcceptor.AcceptCallback", ex);
      }
    }

    public override void Stop()
    {
      if (IdDisposed)
        return;

      try
      {
        Listener.Stop();
        LoggerFacade.LogDebug("TcpAcceptor : Stop accepting");
        accepting = false;
      }
      catch (Exception ex)
      {
        OnSafeException("Exception occurred in TcpAcceptor.Stop", ex);
      }
    }

    protected override void DisposeManagedResources()
    {
      try
      {
        accepting = false;
        Listener.Stop();
      }
      catch (Exception)
      { }
    }

  }
}
