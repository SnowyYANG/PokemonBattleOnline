using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.Commands
{
    public enum ChatMode
    {
        Room,
        Private,
        Public,
    }
    [DataContract(Name = "chat", Namespace = PBOMarks.JSON)]
    public class ChatC2S : IC2S
    {
        public static ChatC2S RoomChat(string chat)
        {
            return new ChatC2S(ChatMode.Room, 0, chat);
        }

        [DataMember(Name = "mode", EmitDefaultValue = false)]
        protected readonly ChatMode Mode;

        [DataMember(Name = "msg")]
        protected readonly string Chat;

        private ChatC2S(ChatMode mode, int to, string chat)
        {
            Mode = mode;
            Chat = chat;
        }
        protected ChatC2S()
        {
        }
    }
    [DataContract(Name = "chat", Namespace = PBOMarks.JSON)]
    public class ChatS2C : IS2C
    {
        [DataMember(Name = "mode", EmitDefaultValue = false)]
        private readonly ChatMode Mode;

        /// <summary>
        /// private mode only
        /// </summary>
        [DataMember(Name = "from", EmitDefaultValue = false)]
        private readonly string From;

        [DataMember(Name = "msg")]
        private readonly string Chat;

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
