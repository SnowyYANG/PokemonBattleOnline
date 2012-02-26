using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging.Lobby;

namespace LightStudio.PokemonBattle.Messaging
{
    public class ChatMessageReceivedEventArgs : EventArgs
    {
        public User UserInfo
        { get; private set; }

        public string Content
        { get; private set; }

        public ChatMessageReceivedEventArgs(User userInfo, string content)
        {
            this.UserInfo = userInfo;
            this.Content = content;
        }
    }
}
