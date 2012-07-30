using System;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging.Primitive
{
    public interface IMessager : IDisposable
    {
        event EventHandler<MessageExceptionEventArgs> UnhandledException;
        event EventHandler<MessageEventArgs> Received;
        void Send(IMessage message);
        void StartReceive();
    }
}
