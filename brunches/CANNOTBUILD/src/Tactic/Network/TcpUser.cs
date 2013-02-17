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
    public event Action Disconnect;
    public readonly TcpServer Server;
    private readonly Socket Socket;
    private readonly TcpPackSender Sender;
    private readonly TcpPackReceiver Receiver;

    public TcpUser(TcpServer server, Socket socket)
    {
      if (!server.ReceiveAsyncEventArgsPool.TryPop(out sendE)) sendE = new TcpPackAsyncEventArgs();
      if (!server.ReceiveAsyncEventArgsPool.TryPop(out receiveE)) receiveE = new TcpPackAsyncEventArgs();
      Server = server;
      Socket = socket;
      Sender = new TcpPackSender(socket, sendE);
      Receiver = new TcpPackReceiver(socket, receiveE);
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
          Server.ReceiveAsyncEventArgsPool.Push(sendE);
          sendE = null;
          Server.ReceiveAsyncEventArgsPool.Push(receiveE);
          receiveE = null;
          Socket.Close(5);
        }
        Socket.Dispose();
      }
    }
  }
}
