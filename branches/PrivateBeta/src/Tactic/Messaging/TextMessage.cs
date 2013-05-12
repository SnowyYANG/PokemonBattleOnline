using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace LightStudio.Tactic.Messaging
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class TextMessage : IMessage
  {
    private string content;

    [DataMember]
    public string Header
    { get; set; }

    [DataMember]
    public string Content
    {
      get { return content; }
      set { content = value ?? string.Empty; }
    }

    public TextMessage()
    {
      content = string.Empty;
    }

    public TextMessage(string header, string content)
    {
      Header = header;
      Content = content;
    }

    public TextMessage(string header, Action<BinaryWriter> buildContent = null)
    {
      Header = header;
      if (buildContent != null)
        using (var stream = new MemoryStream())
        {
          var writer = new BinaryWriter(stream);
          buildContent(writer);
          Content = Convert.ToBase64String(stream.ToArray());
        }
    }

    public void Resolve(Action<BinaryReader> resolveContent)
    {
      using (var stream = new MemoryStream(Convert.FromBase64String(Content)))
      {
        var reader = new BinaryReader(stream);
        resolveContent(reader);
      }
    }
  }
}
