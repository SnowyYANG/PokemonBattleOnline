using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Messaging
{
    internal interface IConnector : IDisposable
    {
        void Connect();
        void Disconnect();
        bool IsConnected { get; }
        IMessager Messager { get; }
        event EventHandler Connected;
        event EventHandler<MessageExceptionEventArgs> ConnectFailed;
        event EventHandler Disconnected;
    }
}
