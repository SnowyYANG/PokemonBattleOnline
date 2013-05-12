using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LightStudio.Tactic.Messaging.Primitive;

namespace LightStudio.Tactic.Messaging
{
  public static class Factory
  {
    public static IMessageClient TcpMessageClient(IPAddress serverAddress, int serverPort)
    {
      return new TcpMessageClient(serverAddress, serverPort);
    }
    public static IMessageServer TcpMessageServer(int port)
    {
      return new TcpMessageServer(port);
    }
  }
}
