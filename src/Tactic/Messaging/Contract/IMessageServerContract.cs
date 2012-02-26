using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
    [ContractClassFor(typeof(IMessageServer))]
    abstract class IMessageServerContract : IMessageServer
    {
        public ICollection<MessageSession> Sessions
        {
            get { return default(ICollection<MessageSession>); }
        }

        public void Start()
        { }

        public void Stop()
        { }

        public void EndSession(int sessionId)
        { }

        public void EndAllSessions()
        { }

        public abstract event EventHandler<SessionCreatedEventArgs> SessionCreated;

        public abstract event EventHandler<SessionEndedEventArgs> SessionEnded;

        public abstract event EventHandler<MessageExceptionEventArgs> UnhandledException;

        public void Broadcast(IMessage message)
        { }

        public bool Send(int messagerId, IMessage message)
        {
            return default(bool);
        }

        public abstract event EventHandler<IdMessageEventArgs> Received;

        public void Dispose()
        { }
    }
}
