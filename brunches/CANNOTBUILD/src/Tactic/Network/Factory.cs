using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Tactic.Network
{
  public static class Factory
  {
    public static INetworkClient TryTcpConnect(IPAddress address, int port)
    {
      return Tcp.TcpClient.TryConnect(address, port);
    }
    public static INetworkServer NewTcpServer(int port)
    {
      return new Tcp.TcpServer(port);
    }
  }
}
