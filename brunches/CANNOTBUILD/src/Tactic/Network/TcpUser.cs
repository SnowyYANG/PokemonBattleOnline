using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace PokemonBattleOnline.Tactic.Network.Tcp
{
  internal class TcpUser : INetworkUser
  {
    public readonly TcpServer Server;
    private readonly Socket Socket;
    private readonly TcpPackSender Sender;
    private readonly TcpPackReceiver Receiver;

    public TcpUser(TcpServer server, Socket socket)
    {
      Server = server;
      Socket = socket;
      Sender = new TcpPackSender(socket);
      Receiver = new TcpPackReceiver(socket);
    }

    public event Action Disconnect
    {
      add { Sender.Disconnect += value; }
      remove { Receiver.Disconnect += value; }
    }
    public int Id
    { get { return ((IPEndPoint)Socket.LocalEndPoint).Port; } }
    public IPEndPoint EndPoint
    { get { return (IPEndPoint)Socket.RemoteEndPoint; } }
    public IPackReceivedListener Listener
    {
      get { return Receiver.Listener; }
      set { Receiver.Listener = value; }
    }

    public void Send(byte[] pack)
    {
      Sender.Send(pack);
    }
    private bool _isDisposed;
    public void Dispose()
    {
      lock (this) //IGNORE: lock (this) is a problem if the instance can be accessed publicly.
      {
        if (!_isDisposed)
        {
          _isDisposed = true;
          Socket.Close(5);
        }
        Socket.Dispose();
      }
    }
  }
}
