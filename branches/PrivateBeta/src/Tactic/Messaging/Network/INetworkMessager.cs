using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace LightStudio.Tactic.Messaging.Primitive
{
    internal interface INetworkMessager : IMessager
    {
        IPAddress RemoteAddress { get; }
        bool Receiving { get; }
    }
}
