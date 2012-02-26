using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.Tactic.Messaging
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
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
  }
}
