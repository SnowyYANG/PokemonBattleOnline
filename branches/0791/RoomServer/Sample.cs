//语言 : CSharp
//入口类 : CodeSample.SampleClass
//引用程序集 : System.dll(默认添加,无需填写)

using System;
namespace CodeSample
{
    public class SampleClass
    {
        private Action<string> _broadcast;
        private Action<string[]> _sendMessage;

        private string[] _banList;

        public SampleClass()
        {
            LoadBanList();
        }
        private void LoadBanList()
        {
            _banList = System.IO.File.ReadAllLines("此处用IP列表文件路径替换,请注意C#中文件路径需用两个斜杠,如C:\\dir\\ban.txt");
        }
        public bool VerifyUser(string user, string address, ref string message)
        {
            if (Array.IndexOf(_banList, address) != -1)
            {
                message = "此处用提示信息替换";
                return false;
            }
            else
            {
                return true;
            }
        }
        public void SetCallback(Action<string> broadcast, Action<string[]> sendMessage)
        {
            _broadcast += broadcast;
            _sendMessage += sendMessage;
        }
        public bool ParseMessage(int id, string message)
        {
            return false;
        }
    }
}
