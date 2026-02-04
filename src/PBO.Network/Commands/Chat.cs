using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonBattleOnline.Network.Commands
{
    public enum ChatMode
    {
        [EnumMember(Value = "room")]
        Room,
        [EnumMember(Value = "private")]
        Private,
        [EnumMember(Value = "public")]
        Public,
    }
    public class ChatC2S : IC2S
    {
        public static ChatC2S RoomChat(string chat)
        {
            return new ChatC2S(ChatMode.Room, 0, chat);
        }

        [JsonPropertyName("mode")]
        public ChatMode Mode {get;set;}

        [JsonPropertyName("msg")]
        public string Chat {get;set;}

        private ChatC2S(ChatMode mode, int to, string chat)
        {
            Mode = mode;
            Chat = chat;
        }
        protected ChatC2S()
        {
        }
    }
    public class ChatS2C : IS2C
    {
        public string type => "chat";
        
        [JsonPropertyName("mode")]
        public ChatMode Mode {get;set;}

        [JsonPropertyName("from")]
        public string From {get;set;}

        [JsonPropertyName("msg")]
        public string Chat {get;set;}

        public ChatS2C(ChatMode mode, string from, string chat)
        {
            Mode = mode;
            From = from;
            Chat = chat;
        }

        void IS2C.Execute(PboClient client)
        {
            var user = client.RoomController.GetUser(From);
            if (user != null)
                switch (Mode)
                {
                    case ChatMode.Public:
                        break;
                    case ChatMode.Room:
                        RoomController.OnRoomChat(Chat, user);
                        break;
                    case ChatMode.Private:
                        break;
                }
        }
    }
}
