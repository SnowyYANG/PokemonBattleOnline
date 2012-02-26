using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Messaging
{
    internal class WriteMessageResult : OperationResult<bool>
    {
        public WriteMessageResult(bool success)
            : base(success)
        { }
        public WriteMessageResult(Exception exception)
            : base(exception)
        { }
    }
}
