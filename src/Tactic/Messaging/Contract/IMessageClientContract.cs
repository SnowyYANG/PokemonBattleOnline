using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
    [ContractClassFor(typeof(IMessageClient))]
    abstract class IMessageClientContract : IMessageClient
    {
        public bool IsConnected
        {
            get { return default(bool); }
        }

        public abstract event EventHandler Connected;

        public abstract event EventHandler<MessageExceptionEventArgs> ConnectFailed;

        public abstract event EventHandler Disconnected;

        public void Connect()
        { }

        public void Disconnect()
        { }

        public abstract event EventHandler<MessageExceptionEventArgs> UnhandledException;

        public abstract event EventHandler<MessageEventArgs> Received;

        public abstract void Send(IMessage message);

        public abstract void StartReceive();

        public abstract void Dispose();
    }
}
