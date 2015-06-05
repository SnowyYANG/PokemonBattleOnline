using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace PokemonBattleOnline.Network
{
  internal class TcpUser : IDisposable
  {
    private event Action _disconnected;
    /// <summary>
    /// always one handler, no need to remove handler
    /// </summary>
    public event Action Disconnected
    { 
      add { _disconnected = value; }
      remove { }
    }

    public readonly TcpServer Server;
    private readonly Socket Socket;
    public readonly TcpPackSender Sender;
    private readonly TcpPackReceiver Receiver;

    public TcpUser(int id, TcpServer server, Socket socket)
    {
      _id = id;
      Server = server;
      Socket = socket;
      Sender = new TcpPackSender(socket);
      Receiver = new TcpPackReceiver(socket);
      Sender.Disconnect += OnDisconnect;
      Receiver.Disconnect += OnDisconnect;
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

    private readonly object Locker = new object();

    private bool _isDisconnected;
    internal void OnDisconnect()
    {
      lock (Locker)
      {
        if (!_isDisconnected)
        {
          try
          {
            Socket.Close(5);
            Socket.Dispose();
            Sender.Disconnect += delegate { };
            Sender.Disconnect -= OnDisconnect;
            Receiver.Disconnect += delegate { };
            Receiver.Disconnect -= OnDisconnect;
            _isDisconnected = true;
            _disconnected();
          }
          catch { }
        }
      }
    }

    private bool _isDisposed;
    public void Dispose()
    {
      lock (Locker)
      {
        if (!_isDisposed)
        {
          OnDisconnect();
          Server.Remove(this);
          _isDisposed = true;
        }
      }
    }
  }
}
