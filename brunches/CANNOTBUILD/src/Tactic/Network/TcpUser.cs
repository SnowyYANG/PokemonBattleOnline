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
    private readonly TcpServer Server;
    private readonly Socket Socket;
    private readonly TcpPackReceiver Receiver;

    public TcpUser(TcpServer server, Socket socket)
    {
      Server = server;
      Socket = socket;
      Receiver = new TcpPackReceiver(socket);
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
      TcpPackSender.Send(Socket, pack);
    }
    public void Dispose()
    {
      Socket.Dispose();
    }
  }
}
