using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace PokemonBattleOnline.Tactic.Network
{
  internal static class TcpPackSender
  {
    public static void Send(Socket socket, byte[] pack)
    {
      int header = pack.Length;
      if (header < 0x8000)
      {
        byte[] h = header < 0x80 ? new byte[] { (byte)(header | 0x80) } : new byte[] { (byte)(header >> 8), (byte)header };
        socket.BeginSend(new ArraySegment<byte>[] { new ArraySegment<byte>(h), new ArraySegment<byte>(pack) }, SocketFlags.None, delegate { }, null);
      }
    }
  }
  /// <summary>
  /// 给我单线程！
  /// </summary>
  internal class TcpPackReceiver
  {
    private readonly Socket Socket;
    private byte[] buffer;
    
    public TcpPackReceiver(Socket socket)
    {
      Socket = socket;
      buffer = new byte[1024];
      BeginReceivePack();
    }

    private IPackReceivedListener _listener;
    public IPackReceivedListener Listener
    {
      get { return _listener ?? NullPackReceivedListener.I; }
      set
      {
        lock (this)
        {
          _listener = value;
          BeginReceivePack();
        }
      }
    }

    private void BeginReceivePack()
    {
      if (_listener != null)
      {
        current = 0;
        BeginReceive(1, EndingReceivePackHeader);
      }
    }

    private byte shortPack;
    private void CheckSize(int size)
    {
      if (buffer.Length < size)
      {
        var l = size << 1;
        if (l > 0x7fff) l = 0x7fff;
        buffer = new byte[l];
      }
    }

    private int current;
    private int size;
    private void BeginReceive(int size, AsyncCallback callback)
    {
      Socket.BeginReceive(buffer, current, size - current, SocketFlags.None, EndingReceivePackHeader, null);
    }
    private void EndingReceivePackHeader(IAsyncResult ar)
    {
      SocketError error;
      var done = Socket.EndReceive(ar, out error);
      if (error == SocketError.Success)
      {
        current += done;
        if (current == 1)
        {
          if ((buffer[0] >> 7) == 1)
          {
            current = 0;
            BeginReceive(buffer[0] & 0x7f, EndingReceivePackContent);
          }
          else BeginReceive(2, EndingReceivePackHeader);
        }
        else
        {
          int size = (buffer[0] << 8) | buffer[1];
          CheckSize(size);
          current = 0;
          BeginReceive(size, EndingReceivePackContent);
        }
      }
    }
    private void EndingReceivePackContent(IAsyncResult ar)
    {
      SocketError error;
      var done = Socket.EndReceive(ar, out error);
      if (error == SocketError.Success)
      {
        current += done;
        if (current < size) BeginReceive(size, EndingReceivePackContent);
        else
        {
          Listener.OnPackReceived(new ArraySegment<byte>(buffer, 0, current).Array.ToArray());
          BeginReceivePack();
        }
      }
    }
  }
}
