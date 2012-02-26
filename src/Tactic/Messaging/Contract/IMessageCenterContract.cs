using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
    [ContractClassFor(typeof(IMessageCenter))]
    abstract class IMessageCenterContract : IMessageCenter
    {
        public void Broadcast(IMessage message)
        {
            Contract.Requires(message != null);
        }

        public bool Send(int messagerId, IMessage message)
        {
            Contract.Requires(message != null);
            return default(bool);
        }

        public abstract event EventHandler<IdMessageEventArgs> Received;

        public abstract void Dispose();

        public abstract event EventHandler<MessageExceptionEventArgs> UnhandledException;
    }
}
