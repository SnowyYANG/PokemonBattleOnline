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
    public event Action<INetworkUser> NewUser;

    private readonly IdGenerator idGen;
    private readonly TcpListener listener;
    private readonly ConcurrentDictionary<int, TcpUser> users;

    public TcpServer(int port)
    {
      idGen = new IdGenerator();
      listener = new TcpListener(IPAddress.Any, port);
    }

    public IPEndPoint ListenerEndPoint
    { get { return (IPEndPoint)listener.LocalEndpoint; } }
    public IEnumerable<INetworkUser> Users
    { get { return users.Values; } }
    private bool _canAddUser;
    public bool CanAddUser
    {
      get { return _canAddUser; }
      set
      {
        lock (listener)
        {
          _canAddUser = value;
          if (value)
          {
            listener.Start();
            WaitForNextUser();
          }
          else
          {
            listener.Stop();
          }
        }
      }
    }

    private void WaitForNextUser()
    {
      if (CanAddUser) listener.BeginAcceptSocket(EndingAcceptSocket, null);
    }
    private void EndingAcceptSocket(IAsyncResult result)
    {
      try
      {
        var s = listener.EndAcceptSocket(result);
        s.LingerState = new LingerOption(true, 10);
        var id = idGen.NextId();
        var user = new TcpUser(this, id, s);
        users[id] = user;
        NewUser(user);
      }
      finally
      {
        WaitForNextUser();
      }
    }

    private TcpUser GetUser(int id)
    {
      return users.ValueOrDefault(id);
    }
    INetworkUser INetworkServer.GetUser(int id)
    {
      return GetUser(id);
    }
    public void RemoveUser(int id)
    {
      TcpUser u;
      users.TryRemove(id, out u);
      u.Dispose();
    }

    public void Dispose()
    {
      CanAddUser = false;
      users.Clear();
      var us = users.Values;
      foreach (var u in us) u.Dispose();
    }
  }
}
