using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Messaging
{
    public class TcpMessageServer : MessageServer
    {
        public TcpMessageServer(int port)
            : this(port, new TextInterpreter())
        { }

        internal TcpMessageServer(int port, IInterpreter interpreter)
            : base(new TcpAcceptor(port, interpreter))
        { }

        internal TcpMessageServer(int port, int idBase)
            : this(port, idBase, new TextInterpreter())
        { }

        internal TcpMessageServer(int port, int idBase, IInterpreter interpreter)
            : base(new TcpAcceptor(port, interpreter), idBase)
        { }
    }
}
