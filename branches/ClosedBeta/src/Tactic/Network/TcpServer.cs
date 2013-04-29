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
    private static void OnKeepAlive(object state)
    {
      var users = (List<TcpUser>)state;
      var lastPack = DateTime.Now.AddMilliseconds(-2d * PBOMarks.TIMEOUT);
      foreach (var u in users.ToArray())
        if (u.LastPack < lastPack) u.Dispose();
    }
    
    public event Action<INetworkUser> NewComingUser;

    public readonly IdsPool IdsPool;
    private readonly List<TcpUser> Users;
    private readonly int Port;
    private readonly Timer KeepAliveTimer;
    private readonly object ListenerLocker;

    public TcpServer(int port)
    {
      IdsPool = new IdsPool();
      Users = new List<TcpUser>(300);
      Port = port;
      KeepAliveTimer = new Timer(OnKeepAlive, Users, PBOMarks.TIMEOUT << 1, PBOMarks.TIMEOUT << 1);
      ListenerLocker = new object();
    }

    private Socket listener;
    private bool _isListening;
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
              listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
              listener.Bind(new IPEndPoint(IPAddress.Any, Port));
              listener.Listen(32);
              StartAccept(null);
            }
            else
            {
              listener.Close();
              listener.Dispose();
              listener = null;
            }
          }
        }
      }
    }
    public IPEndPoint ListenerEndPoint
    { get { return (IPEndPoint)listener.LocalEndPoint; } }

    private void StartAccept(SocketAsyncEventArgs acceptEventArg)
    {
      if (acceptEventArg == null)
      {
        acceptEventArg = new SocketAsyncEventArgs();
        acceptEventArg.Completed += ProcessAccept;
      }
      else acceptEventArg.AcceptSocket = null;
      bool willRaiseEvent = listener.AcceptAsync(acceptEventArg);
      if (!willRaiseEvent) ProcessAccept(null, acceptEventArg);
    }
    private void ProcessAccept(object sender, SocketAsyncEventArgs e)
    {
      Socket s = e.AcceptSocket;
      s.LingerState = new LingerOption(true, 5);
      var u = new TcpUser(this, s);
      Users.Add(u); //thread safe?
      NewComingUser(u);
      if (IsListening) StartAccept(e);
    }

    internal void Remove(TcpUser user)
    {
      Users.Remove(user); //thread safe?
      IdsPool.Push(user.Id);
    }

    public void Dispose()
    {
      NewComingUser = delegate { };
      IsListening = false;
    }
  }
}
