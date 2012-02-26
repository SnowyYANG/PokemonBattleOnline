using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Room
{
  public enum MessageTarget
  {
    All,
    Team,
    User
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class ChatMessage
  {
    [DataMember(EmitDefaultValue = false)]
    public MessageTarget Target
    { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public int TargetId
    { get; set; }

    [DataMember]
    public string Content
    { get; set; }

    public ChatMessage()
    { }

    public ChatMessage(MessageTarget target, string content)
    {
      this.Target = target;
      this.Content = content;
    }

    public ChatMessage(MessageTarget target, int targetId, string content)
    {
      this.Target = target;
      this.TargetId = targetId;
      this.Content = content;
    }
  }
}
