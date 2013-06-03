using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    class Command
    {
        private Action<string> _broadcast;
        private Action<string[]> _send;
        public void SetCallback(Action<string> callback1, Action<string[]> callback2)
        {
            _broadcast += callback1;
            _send += callback2;
        }
        public bool VerifyUser(string name, string address, ref string message)
        {
            if (name == "fa")
            {
                message = "you are ban.";
                return false;
            }
            return true;
        }
        public bool ParseMessage(int identity, string message)
        {
            if (message == "/s")
            {
                _broadcast("stop");
                _send(new string[] { identity.ToString(), "you have to stop!" });
                return true;
            }
            return false;
        }
    }
}
