using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using LightStudio.Tactic;

namespace LightStudio.Tactic.Messaging
{
  internal class DataContractResolver : IMessageResolver
  {
    public IMessagable GetMessageObject(IMessage message)
    {
      var type = Type.GetType(message.Header);
      if (type != null) return (IMessagable)Serializer.DeserializeFromString(type, message.Content);
      return null;
    }

    public IMessage ToMessage(IMessagable obj)
    {
      var type = obj.GetType();
      return new TextMessage(type.AssemblyQualifiedName, Serializer.SerializeToString(obj));
    }
  }
}
