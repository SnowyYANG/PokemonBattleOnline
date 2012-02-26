using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Messaging
{
    internal interface IAcceptor : IDisposable
    {
        bool Accepting { get; }
        void Start();
        void Stop();
        event EventHandler<MessagerEventArgs> Accepted;
        event EventHandler<MessageExceptionEventArgs> UnhandledException;
    }
}
