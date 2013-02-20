using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace PokemonBattleOnline.Tactic.Network.Tcp
{
  internal class TcpServer : INetworkServer
  {
    public event Action<INetworkUser> NewComingUser;

    private readonly int Port;
    private readonly object ListenerLocker;
    private Socket Listener;

    public TcpServer(int port)
    {
      Port = port;
      ListenerLocker = new object();
    }

    public IPEndPoint ListenerEndPoint
    { get { return (IPEndPoint)Listener.LocalEndPoint; } }
    private volatile bool _isListening;
    public bool IsListening
    {
      get
      {
        lock (ListenerLocker)
        {
          return _isListening;
        }
      }
      set
      {
        lock (ListenerLocker)
        {
          if (_isListening != value)
          {
            _isListening = value;
            if (value)
            {
              Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
              Listener.Bind(new IPEndPoint(IPAddress.Any, Port));
              Listener.Listen(32);
              StartAccept(null);
            }
            else
            {
              Listener.Close();
              Listener.Dispose();
              Listener = null;
            }
          }
        }
      }
    }

    private void StartAccept(SocketAsyncEventArgs acceptEventArg)
    {
      if (acceptEventArg == null)
      {
        acceptEventArg = new SocketAsyncEventArgs();
        acceptEventArg.Completed += ProcessAccept;
      }
      else acceptEventArg.AcceptSocket = null;
      bool willRaiseEvent = Listener.AcceptAsync(acceptEventArg);
      if (!willRaiseEvent) ProcessAccept(null, acceptEventArg);
    }
    private void ProcessAccept(object sender, SocketAsyncEventArgs e)
    {
      Socket s = e.AcceptSocket;
      s.LingerState = new LingerOption(true, 5);
      NewComingUser(new TcpUser(this, s));
      if (_isListening) StartAccept(e);
    }

    public void Dispose()
    {
      NewComingUser = delegate { };
      IsListening = false;
    }
  }
}
