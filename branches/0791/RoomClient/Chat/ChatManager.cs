using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBattle.RoomClient
{
    public delegate void ChatDelegate(int identity);
    public class ChatManager
    {
        private Dictionary<int, ChatForm> _chats = new Dictionary<int, ChatForm>();
        private Dictionary<int, List<string>> _waitChats = new Dictionary<int, List<string>>();

        #region Events
        public event ChatDelegate OnAddChat;

        private void HandleOnAddChatEvent(int from)
        {
            if (OnAddChat != null) OnAddChat(from);
        }

        public event ChatDelegate OnRemoveChat;

        private void HandleOnRemoveChatEvent(int target)
        {
            if (OnRemoveChat != null) OnRemoveChat(target);
        }
        #endregion

        public void PassChatMessage(int from, string message)
        {
            if (_chats.ContainsKey(from))
            {
                _chats[from].ReceiveChatMessage(message);
            }
            else
            {
                if (!_waitChats.ContainsKey(from))
                {
                    _waitChats[from] = new List<string>();
                    HandleOnAddChatEvent(from);
                }
                _waitChats[from].Add(message);
            }
        }

        public bool BuildChatForm(int target,string myName,RoomClient client)
        {
            if (_chats.ContainsKey(target)) return false;

            ChatForm newForm = new ChatForm(target, myName, client);
            _chats[target] = newForm;
            newForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ChatForm_FormClosed);

            if (_waitChats.ContainsKey(target))
            {
                List<string> messageList = _waitChats[target];

                _waitChats.Remove(target);
                HandleOnRemoveChatEvent(target);
                foreach (string message in messageList)
                {
                    newForm.ReceiveChatMessage(message);
                }
            }
            
            return true;
        }

        void ChatForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            int target = (sender as ChatForm).Target;
            _chats.Remove(target);
        }

        public ChatForm GetChatForm(int target)
        {
            if (_chats.ContainsKey(target))
            {
                return _chats[target];
            }
            return null;
        }
    }
}
