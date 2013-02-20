using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Tactic.Network
{
  public static class ClientFactory
  {
    public static void TryTcpConnect(IPAddress address, int port, Action<INetworkClient> callback)
    {
      try
      {
        Tcp.TcpClient.BeginConnect(address, port, callback);
      }
      catch
      {
        callback(null);
      }
    }
    public static void TryTcpConnect(string address, int port, Action<INetworkClient> callback)
    {
      try
      {
        Tcp.TcpClient.BeginConnect(address, port, callback);
      }
      catch
      {
        callback(null);
      }
    }
  }

  public static class ServerFactory
  {
    public static INetworkServer NewTcpServer(int port)
    {
      return new Tcp.TcpServer(port);
    }
  }
}
