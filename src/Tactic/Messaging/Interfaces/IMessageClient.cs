﻿using System;
using System.Diagnostics.Contracts;

namespace LightStudio.Tactic.Messaging.Primitive
{
    public interface IMessageClient : IMessager
    {
        bool IsConnected { get; }
        event EventHandler Connected;
        event EventHandler<MessageExceptionEventArgs> ConnectFailed;
        event EventHandler Disconnected;
        void Connect();
        void Disconnect();
    }
}
