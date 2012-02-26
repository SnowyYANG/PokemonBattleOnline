using System;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging
{
    [ContractClass(typeof(IMessageCenterContract))]
    public interface IMessageCenter : IDisposable
    {
        void Broadcast(IMessage message);
        bool Send(int messagerId, IMessage message);

        event EventHandler<IdMessageEventArgs> Received;
        event EventHandler<MessageExceptionEventArgs> UnhandledException;
    }
}
