using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
  internal class TcpMessager : NetworkMessager
  {
    private BinaryReader reader;
    private BinaryWriter writer;
    protected readonly TcpClient TcpClient;
    protected readonly IInterpreter Interpreter;
    public override IPAddress RemoteAddress
    {
      get
      {
        return (TcpClient.Client.RemoteEndPoint as IPEndPoint).Address;
      }
    }

    public TcpMessager(TcpClient client, IInterpreter interpreter)
    {
      Contract.Requires(client != null);
      Contract.Requires(interpreter != null);

      this.TcpClient = client;
      this.Interpreter = interpreter;

      var stream = client.GetStream();
      this.writer = new BinaryWriter(stream);
      this.reader = new BinaryReader(stream);
    }

    private void ReceiveNext()
    {
      try
      {
        Interpreter.ReadAsync(reader, ReceiveCallback);
      }
      catch (Exception ex)
      {
        OnSafeException("Exception occurred in TcpMessager.RecevieNext", ex);
      }
    }

    private void ReceiveCallback(ReadMessageResult result)
    {
      if (result.IsCompleted)
      {
        OnSafeReceive(result.Result);
        if (!IdDisposed)
          ReceiveNext();
      }
      else
      {
        if (result.Exception != null)
          OnSafeException("Exception occurred in TcpMessager.Receive", result.Exception);
      }
    }

    protected override void StartReceiveInternal()
    {
      ReceiveNext();
    }

    protected override void SendInternal(IMessage message)
    {
      var result = Interpreter.Write(message, writer);
      if (result.Exception != null)
        OnSafeException("Exception occurred in TcpMessager.Send", result.Exception);
    }

    protected override void DisposeManagedResources()
    {
      TcpClient.Close();
      writer.Close();
      reader.Close();
      base.DisposeManagedResources();
    }
  }
}
