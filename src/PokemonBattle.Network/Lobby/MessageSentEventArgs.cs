using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;

namespace LightStudio.PokemonBattle.Messaging
{
    public class MessageSentEventArgs : EventArgs
    {
        public int[] Receivers
        { get; private set; }

        public IMessage Message
        { get; private set; }

        public MessageSentEventArgs(int[] receivers, IMessage message)
        {
            this.Receivers = receivers;
            this.Message = message;
        }
    }
}
