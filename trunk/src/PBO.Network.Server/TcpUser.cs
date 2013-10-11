using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace PokemonBattleOnline.Network
{
  internal class TcpUser
  {
    public readonly TcpServer Server;
    private readonly Socket Socket;
    public readonly TcpPackSender Sender;
    private readonly TcpPackReceiver Receiver;
    private readonly object Locker;

    public TcpUser(TcpServer server, Socket socket)
    {
      Server = server;
      Socket = socket;
      Sender = new TcpPackSender(socket);
      Receiver = new TcpPackReceiver(socket);
      Locker = new object();
      _id = server.IdsPool.GetId();
    }

    /// <summary>
    /// 妥妥的执行两次
    /// </summary>
    public event Action Disconnect
    {
      add
      { 
        Sender.Disconnect += value;
        Receiver.Disconnect += value;
      }
      remove
      {
        Sender.Disconnect -= value;
        Receiver.Disconnect -= value;
      }
    }
    private readonly int _id;
    public int Id
    { get { return _id; } }
    public IPEndPoint EndPoint
    { get { return (IPEndPoint)Socket.RemoteEndPoint; } }
    public IPackReceivedListener Listener
    {
      get { return Receiver.Listener; }
      set { Receiver.Listener = value; }
    }
    internal DateTime LastPack
    { get { return Receiver.LastPack; } }

    private bool _isDisposed;
    public void Dispose()
    {
      lock (Locker)
      {
        if (!_isDisposed)
        {
          _isDisposed = true;
          Server.Remove(this);
          try
          {
            Socket.Close(5);
            Socket.Dispose();
          }
          catch { }
        }
      }
    }
  }
}
