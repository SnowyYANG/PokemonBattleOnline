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

    private readonly TcpListener listener;

    public TcpServer(int port)
    {
      listener = new TcpListener(IPAddress.Any, port);
    }

    public IPEndPoint ListenerEndPoint
    { get { return (IPEndPoint)listener.LocalEndpoint; } }
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
        var user = new TcpUser(this, s);
        NewComingUser(user);
      }
      finally
      {
        WaitForNextUser();
      }
    }

    public void Dispose()
    {
      CanAddUser = false;
    }
  }
}
