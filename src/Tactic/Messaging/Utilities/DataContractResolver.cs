using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;
using LightStudio.Tactic;

namespace LightStudio.Tactic.Messaging
{
  internal class DataContractResolver : IMessageResolver
  {
    public IMessagable GetMessageObject(IMessage message)
    {
      var type = Type.GetType(message.Header);
      if (type != null)
      {
        var serializer = Serializer.GetSerializer(type);
        using (var reader = XmlReader.Create(new StringReader(message.Content)))
        {
          return (IMessagable)serializer.ReadObject(reader);
        }
      }
      return null;
    }

    public IMessage ToMessage(IMessagable obj)
    {
      var type = obj.GetType();
      var serializer = Serializer.GetSerializer(type);
      var sb = new StringBuilder();
      using (var writer = XmlWriter.Create(sb))
      {
        serializer.WriteObject(writer, obj);
      }
      return new TextMessage(type.AssemblyQualifiedName, sb.ToString());
    }
  }
}
