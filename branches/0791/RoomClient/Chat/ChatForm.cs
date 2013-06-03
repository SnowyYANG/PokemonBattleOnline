using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PokemonBattle.BattleRoom.Client;
using NetworkLib;

namespace PokemonBattle.RoomClient
{
    public partial class ChatForm : Form
    {


        private int _target;
        private string _myName;
        private RoomClient _client;

        public ChatForm(int target,string myName ,RoomClient client)
        {
            InitializeComponent();
            _target = target;
            _myName = myName;
            _client = client;
            MessageText.KeyDown += new KeyEventHandler(MessageText_KeyDown);
        }

        void MessageText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendButton.PerformClick();
            }
        }

        public void ReceiveChatMessage(string message)
        {
            AppendText(message);
        }

        private void AppendText(string text)
        {
            if (!InvokeRequired)
            {
                if (DisplayText.TextLength > 0)
                {
                    DisplayText.Text += "\r\n";
                }
                DisplayText.Text += String.Format("{0}", text);

                DisplayText.SelectionStart = DisplayText.Text.Length - 1;
                DisplayText.ScrollToCaret();
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { AppendText(text); }));
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (RoomClientForm.IsEmptyString(MessageText.Text)) return;
            string message = string.Format("{0} : {1}", _myName, MessageText.Text);
            if (_client.Chat(_target, message))
            {
                AppendText(message);
                MessageText.Clear();
            }
            else
            {
                AppendText("对方已经不再房间中.");
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        public int Target
        {
            get { return _target; }
        }
    }
}
