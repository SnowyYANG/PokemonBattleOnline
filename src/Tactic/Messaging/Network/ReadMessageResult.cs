using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Messaging.Primitive
{
    internal class ReadMessageResult : OperationResult<IMessage>
    {
        public ReadMessageResult(IMessage message)
            : base(message)
        { }
        public ReadMessageResult(Exception exception)
            : base(exception)
        { }
    }
}
