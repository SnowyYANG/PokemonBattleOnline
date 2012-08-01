using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using User = LightStudio.Tactic.Messaging.User<LightStudio.PokemonBattle.Messaging.UserExtension>;

namespace LightStudio.PokemonBattle.Messaging
{
  public class ChatMessageReceivedEventArgs : EventArgs
  {
    public User UserInfo { get; private set; }
    public string Content { get; private set; }

    public ChatMessageReceivedEventArgs(User userInfo, string content)
    {
      UserInfo = userInfo;
      Content = content;
    }
  }
  public class Lobby : ClientService
  {
    internal Lobby(Client client)
      : base(client, MessageHeaders.CHAT)
    {
    }

    public event EventHandler<ChatMessageReceivedEventArgs> ChatMessageReceived = delegate { };
    public void Chat(string message, params int[] receivers)
    {
      SendMessage(MessageHeaders.CHAT, reader => reader.Write(message), receivers);
    }
    protected override void ReadMessage(User sender, byte header, System.IO.BinaryReader reader)
    {
      ChatMessageReceived(this, new ChatMessageReceivedEventArgs(sender, reader.ReadString()));
    }
  }
}
