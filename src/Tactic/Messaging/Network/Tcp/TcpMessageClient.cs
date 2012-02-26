using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace LightStudio.Tactic.Messaging
{
    public class TcpMessageClient : MessageClient
    {
        public TcpMessageClient(IPAddress hostAddress, int port)
            : this(hostAddress, port, new TextInterpreter())
        { }

        internal TcpMessageClient(IPAddress hostAddress, int port, IInterpreter interpreter)
            : base(new TcpConnector(hostAddress, port, interpreter))
        { }
    }
}
