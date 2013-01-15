using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace PokemonBattleOnline.Tactic.Network
{
  internal class TcpClient : INetworkClient
  {
    public static TcpClient TryConnect(IPAddress address, int port)
    {
      var socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
      try
      {
        socket.Blocking = true;
        socket.LingerState = new LingerOption(true, 5);
        socket.Connect(address, port);
        return new TcpClient(socket);
      }
      catch (Exception e)
      {
        socket.Dispose();
        throw e;
      }
    }
    
    private readonly Socket Socket;
    private readonly TcpPackReceiver Receiver;

    private TcpClient(Socket socket)
    {
      Socket = socket;
      Receiver = new TcpPackReceiver(socket);
    }

    public IPEndPoint Server
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
      Socket.Close();
    }
  }
}
