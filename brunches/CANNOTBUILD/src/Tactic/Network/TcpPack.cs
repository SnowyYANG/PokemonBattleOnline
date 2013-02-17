using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace PokemonBattleOnline.Tactic.Network.Tcp
{
  internal class TcpPackSender
  {
    private static void Completed(object sender, SocketAsyncEventArgs e)
    {
      var s = ((TcpPackSender)e.UserToken);
      lock (s.Locker)
      {
        s.Completed(e);
      }
    }

    public event Action Disconnect;
    
    private readonly Socket Socket;
    private readonly SocketAsyncEventArgs E;
    private readonly object Locker;

    public TcpPackSender(Socket socket)
      : this(socket, new SocketAsyncEventArgs())
    {
      E.Completed += TcpPackSender.Completed;
    }
    public TcpPackSender(Socket socket, SocketAsyncEventArgs e)
    {
      e.UserToken = this;
      Socket = socket;
      E = e;      
      Locker = new object();
    }

    private List<ArraySegment<byte>> Buffer;
    private bool isSending;
    private void SendAsync()
    {
      lock (Locker)
      {
        E.BufferList = Buffer;
        Buffer = null;
        if (!Socket.SendAsync(E)) Completed(E);
        else isSending = true;
      }
    }
    private void Completed(SocketAsyncEventArgs e)
    {
      //already locked
      if (e.SocketError == SocketError.Success)
      {
        if (Buffer == null) isSending = false;
        else SendAsync();
      }
      else Disconnect();
    }

    public void Send(byte[] pack)
    {
      int header = pack.Length;
      if (header < 0x8000)
      {
        lock (Locker)
        {
          if (Buffer == null) Buffer = new List<ArraySegment<byte>>();
          Buffer.Add(new ArraySegment<byte>(header < 0x80 ? new byte[] { (byte)(header | 0x80) } : new byte[] { (byte)(header >> 8), (byte)header }));
          Buffer.Add(new ArraySegment<byte>(pack));
          if (!isSending) SendAsync();
        }
      }
    }
  }
  
  internal class TcpPackReceiver
  {
    private static void Completed(object sender, SocketAsyncEventArgs e)
    {
      ((TcpPackReceiver)e.UserToken).Completed(e);
    }

    public event Action Disconnect;

    private readonly Socket Socket;
    private readonly object Locker;
    private SocketAsyncEventArgs E;
    private byte[] buffer;

    public TcpPackReceiver(Socket socket)
      : this(socket, new SocketAsyncEventArgs())
    {
      E.Completed += TcpPackReceiver.Completed;
    }
    public TcpPackReceiver(Socket socket, SocketAsyncEventArgs e)
    {
      e.UserToken = this;
      Socket = socket;
      Locker = new object();
      E = e;
      buffer = new byte[1024];
    }

    private IPackReceivedListener _listener;
    public IPackReceivedListener Listener
    {
      get { return _listener ?? NullPackReceivedListener.I; }
      set { _listener = value; }
    }

    private byte step;
    private int size;
    private void ReceivePackAsync(int offset, int count)
    {
      E.SetBuffer(buffer, offset, count);
      if (!Socket.ReceiveAsync(E)) Completed(E);
    }
    private void Completed(SocketAsyncEventArgs e)
    {
      var done = e.BytesTransferred;
      if (done > 0 && e.SocketError == SocketError.Success)
      {
        switch (step)
        {
          case 0:
            if ((buffer[0] >> 7) == 1)
            {
              step = 2;
              size = buffer[0] & 0x7f;
              ReceivePackAsync(0, size);
            }
            else
            {
              step = 1;
              ReceivePackAsync(1, 1);
            }
            break;
          case 1:
            step = 2;
            size = (buffer[0] << 8) | buffer[1];
            if (buffer.Length < size)
            {
              var l = size << 1;
              if (l > 0x7fff) l = 0x7fff;
              buffer = new byte[l];
            }
            ReceivePackAsync(0, size);
            break;
          case 2:
            var current = e.Offset + done;
            if (done < e.Count) ReceivePackAsync(current, size - current);
            else
            {
              Listener.OnPackReceived(buffer.SubArray(0, size));
              step = 0;
              ReceivePackAsync(0, 1);
            }
            break;
        }
      }
      else Disconnect();
    }
  }
}
