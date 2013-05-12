using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.Tactic.Messaging
{
  [Serializable]
  public class MessageException : Exception
  {
    public MessageException()
    { }

    public MessageException(string message)
      : base(message)
    { }

    protected MessageException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    { }

    public MessageException(string message, Exception innerException)
      : base(message, innerException)
    { }

    public string InnerMessage
    {
      get
      {
        return InnerException != null ? InnerException.Message : string.Empty;
      }
    }
  }
}
