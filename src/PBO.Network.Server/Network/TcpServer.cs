using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace PokemonBattleOnline.Network
{
  internal class TcpServer
  {
    private static void OnKeepAlive(object state)
    {
#if !TEST
      var server = (TcpServer)state;
      var lastPack = DateTime.Now.AddMilliseconds(-2d * PBOMarks.TIMEOUT);
      TcpUser[] users;
      lock(server.Locker)
      {
        users = server.Users.ToArray();
      }
      foreach (var u in users)
        if (u.LastPack < lastPack) u.OnDisconnect();
#endif
    }
    
    public event Action<TcpUser> NewComingUser;

    private readonly IdsPool IdsPool;
    private readonly List<TcpUser> Users;
    private readonly int Port;
    private readonly Timer KeepAliveTimer;
    private readonly object ListenerLocker;
    public List<IPAddress> Banlist;

    public TcpServer(int port)
    {
      IdsPool = new IdsPool();
      Users = new List<TcpUser>(100);
      Port = port;
      KeepAliveTimer = new Timer(OnKeepAlive, this, PBOMarks.TIMEOUT << 1, PBOMarks.TIMEOUT << 1);
      ListenerLocker = new object();
      Banlist = new List<IPAddress>();
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
    private readonly object Locker = new object();
    private void ProcessAccept(object sender, SocketAsyncEventArgs e)
    {
      Socket s = e.AcceptSocket;
      IPAddress tip = ((IPEndPoint)s.RemoteEndPoint).Address;
      Console.Write(DateTime.Now.ToString("(hh:mm:ss) "));
      Console.WriteLine(tip.ToString()+" is trying to login.");
      s.LingerState = new LingerOption(true, 5);
      var u = new TcpUser(IdsPool.GetId(), this, s);
      if (Banlist.IndexOf(tip) >= 0)
      {
          u.Dispose();
          if (IsListening) StartAccept(e);
          return;
      }
      lock (Locker)
      {
        Users.Add(u);
      }
      NewComingUser(u);
      if (IsListening) StartAccept(e);
    }
    internal void Remove(TcpUser user)
    {
      lock (Locker)
      {
        Users.Remove(user);
      }
      IdsPool.Push(user.Id);
    }

    public void Dispose()
    {
      NewComingUser = delegate { };
      IsListening = false;
    }
  }
}
