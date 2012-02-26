using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
    [ContractClassFor(typeof(IMessager))]
    abstract class IMessagerContract : IMessager
    {

        public void Send(IMessage message)
        {
            Contract.Requires(message != null);
        }

        public void StartReceive()
        { }

        public void Dispose()
        { }

        public abstract event EventHandler<MessageExceptionEventArgs> UnhandledException;
        public abstract event EventHandler<MessageEventArgs> Received;
    }
}
