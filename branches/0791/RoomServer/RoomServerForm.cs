using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using PokemonBattle.RoomListClient;
using PokemonBattle.RoomList.Client;
using PokemonBattle.BattleRoom.Server;
using NetworkLib;

namespace PokemonBattle.RoomServer
{
    public partial class RoomServerForm : Form
    {
        #region Varibles
        private Room _roomInfo;
        private RoomServer _server;
        private RoomListClient.RoomListClient _listClient;
        private bool _exit;
        private Timer _listTimer;
        #endregion

        public RoomServerForm()
        {
            InitializeComponent();

            _roomInfo = new Room();
            _roomInfo.Name = MySetting.Name;
            _roomInfo.MaxUser = MySetting.MaxUser;
            _roomInfo.Description = MySetting.Description;
            _roomInfo.UserCount = 0;
        }

        #region EventHandler

        private void RoomServerForm_Load(object sender, EventArgs e)
        {
            if (MySetting == null || string.IsNullOrEmpty(MySetting.Name))
            {
                InitSetting();
                if (string.IsNullOrEmpty(MySetting.Name))
                {
                    Close();
                    return;
                }
            }

            Icon = Properties.Resources.PokemonBall;
            RoomIcon.Icon = Properties.Resources.PokemonBall;
            UserList.ListViewItemSorter = new ListViewSorter(0);

            FormClosing += new FormClosingEventHandler(RoomServerForm_FormClosing);
            MessageText.KeyDown += new KeyEventHandler(MessageText_KeyDown);

            RunServer();
            if (MySetting.LogonList && !_exit)
            {
                _listTimer = new Timer();
                _listTimer.Interval = 300000;
                _listTimer.Tick += new EventHandler(_listTimer_Tick);
                ConnectServer();
            }
            if (CodeSetting != null)
            {
                if (CodeSetting.UseRoomCoder && !IsEmptyString(CodeSetting.EntranceClass) && !string.IsNullOrEmpty(CodeSetting.RoomCodePath))
                {
                    RoomCodeHelper.LoadCode(CodeSetting.RoomCodePath);
                }
            }
            RoomCoderForm.RemoveAssemblies();
        }

        private void InitSetting()
        {
            MySetting = new RoomSetting();
            MySetting.BattleSetting.MoveInterval = 120;
            DialogResult result = new RoomSettingForm(MySetting).ShowDialog();
        }

        private void RoomServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_exit)
            {
                CloseListClient();
                CloseRoomServer();
                Properties.Settings.Default.Save();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void SettingMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = new RoomSettingForm(MySetting).ShowDialog();
            if (result == DialogResult.OK)
            {
                _roomInfo.Name = MySetting.Name;
                _roomInfo.Description = MySetting.Description;
                _roomInfo.MaxUser = MySetting.MaxUser;
                if (_listClient != null)
                {
                    _listClient.RoomUpdateInfo();
                }
                if (_server != null)
                {
                    _server.UpdateSetting();
                }
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            _exit = true;
            Close();
        }

        private void KickMenu_Click(object sender, EventArgs e)
        {
            if (UserList.SelectedItems.Count > 0)
            {
                _server.KickUser(Int32.Parse(UserList.SelectedItems[0].Name));
            }
        }

        void MessageText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendButton.PerformClick();
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (!IsEmptyString(MessageText.Text))
            {
                string message = string.Format("房间信息 : {0}", MessageText.Text);
                _server.BroadcastMessage(message);
                MessageText.Clear();
            }
        }

        private void OpenWinMenuItem_Click(object sender, EventArgs e)
        {
            ShowMe();
        }

        private void HideMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void RoomCodeMenuItem_Click(object sender, EventArgs e)
        {
            RoomCoderForm form = new RoomCoderForm();
            form.ShowDialog();
        }
        #endregion

        private void AppendText(string text)
        {
            if (!InvokeRequired)
            {
                if (DisplayText.TextLength > 0)
                {
                    DisplayText.Text += "\r\n";
                }
                DisplayText.Text += String.Format("{0} : {1}", DateTime.Now.ToLongTimeString(), text);

                DisplayText.SelectionStart = DisplayText.Text.Length - 1;
                DisplayText.ScrollToCaret();
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { AppendText(text); }));
            }
        }

        private RoomSetting MySetting
        {
            get
            {
                return Properties.Settings.Default.RoomSetting;
            }
            set
            {
                Properties.Settings.Default.RoomSetting = value;
            }
        }

        private RoomCodeSetting CodeSetting
        {
            get
            {
                return Properties.Settings.Default.CodeSetting;
            }
            set
            {
                Properties.Settings.Default.CodeSetting = value;
            }
        }

        private void ShowMe()
        {
            Show();
            Activate();
        }

        private bool IsEmptyString(string str)
        {
            return Regex.IsMatch(str, "^\\s*$");
        }

        #region RoomServer

        private void RunServer()
        {
            _server = new RoomServer(MySetting.BattleSetting);
            _server.OnAddUserInfo += new UserDelegate(_server_OnAddUser);
            _server.OnRemoveUserInfo += new RemoveUserDelegate(_server_OnRemoveUser);
            _server.OnUpdateUserInfo += new UserDelegate(_server_OnUpdateUserInfo);
            _server.OnVerifyUser += new VerifyUserDelegate(_server_OnVerifyUser);
            _server.OnGetMessage += new MessageDelegate(_server_OnGetMessage);
            if (_server.Initialize())
            {
                _server.RunThread();
                RoomCodeHelper.SetCallback(_server.BroadcastMessage, SendMessage);
                _server.OnRoomCommand += RoomCodeHelper.ParseMessage;
            }
            else
            {
                MessageBox.Show("服务器初始化失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _exit = true;
                Close();
            }
        }

        private void SendMessage(string[] args)
        {
            _server.SendMessage(int.Parse(args[0]),args[1]);
        }

        void _server_OnGetMessage(string message)
        {
            AppendText(message);
        }

        bool _server_OnVerifyUser(User userInfo, out string failMessage)
        {
            failMessage = "";
            if (_roomInfo.UserCount >= _roomInfo.MaxUser)
            {
                failMessage = "房间人数已满";
                return false;
            }
            if (_server.ExistsUser(new Predicate<User>(delegate(User info) { return info.Name == userInfo.Name; })))
            {
                failMessage = "该用户名已被使用";
                return false;
            }
            if (! RoomCodeHelper.VerifyUser(userInfo.Name,userInfo.Address,ref failMessage))
            {
                return false;
            }
            return true;
        }

        void _server_OnUpdateUserInfo(User userInfo)
        {
            if (!InvokeRequired)
            {
                ListViewItem[] item = UserList.Items.Find(userInfo.Identity.ToString(), false);
                if (item.Length > 0)
                {
                    item[0].SubItems[0].Text = userInfo.Name;
                    item[0].SubItems[1].Text = userInfo.Address;
                    item[0].SubItems[2].Text = GetStateString(userInfo.State);
                }
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { _server_OnUpdateUserInfo(userInfo); }));
            }
        }

        void _server_OnRemoveUser(int identity)
        {
            if (!InvokeRequired)
            {
                ListViewItem[] item = UserList.Items.Find(identity.ToString(), false);
                if (item.Length > 0)
                {
                    _roomInfo.UserCount -= 1;

                    UserList.Items.Remove(item[0]);

                    AppendText(string.Format("{0}离开了房间.", item[0].SubItems[0].Text));

                    UpdateUserCount();
                }
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { _server_OnRemoveUser(identity); }));
            }
        }

        void _server_OnAddUser(User userInfo)
        {
            if (!IsEmptyString(MySetting.WelcomeMessage)) _server.SendMessage(userInfo.Identity, MySetting.WelcomeMessage);

            _roomInfo.UserCount += 1;
            ListViewItem item = new ListViewItem(new string[] { userInfo.Name, userInfo.Address, GetStateString(userInfo.State) });
            item.Name = userInfo.Identity.ToString();

            if (InvokeRequired) Invoke(new VoidFunctionDelegate(delegate { UserList.Items.Add(item); }));

            AppendText(string.Format("{0}进入了房间.", userInfo.Name));
            UpdateUserCount();
        }

        private string GetStateString(BattleRoom.Server.UserState state)
        {
            switch (state)
            {
                case UserState.Free:
                    return "空闲";
                case UserState.Battling:
                    return "对战中";
                case UserState.Away:
                    return "离开";
                case UserState.Challenging:
                    return "挑战中";
                default:
                    return "";
            }
            
        }

        private void CloseRoomServer()
        {
            if (_server != null)
            {
                _server.Stop();
                _server = null;
            }
        }

        #endregion

        #region RoomListClient

        private void ConnectServer()
        {
            _listClient = new RoomListClient.RoomListClient(GetServerIP(), _roomInfo);
            _listClient.OnRoomLogoned += new NetworkLib.VoidFunctionDelegate(_listClient_OnRoomLogoned);
            _listClient.OnConnectFail += new NetworkLib.NetworkErrorDelegate(_listClient_OnConnectFail);
            _listClient.OnDisconnected += new NetworkLib.NetworkEventDelegate(_listClient_OnDisconnected);
            _listClient.OnRoomLogonFailed += new LogonFailDelegate(_listClient_OnRoomLogonFailed);

            _listClient.Initialize();
            _listClient.RunThread();
            _listTimer.Stop();
        }

        void _listClient_OnRoomLogoned()
        {
            AppendText("已登录房间列表服务器.");
        }

        void _listClient_OnRoomLogonFailed(string message)
        {
            AppendText(String.Format("无法登录房间列表服务器, {0}.", message));
        }

        void _listClient_OnDisconnected()
        {
            AppendText("与房间列表服务器断开了连接.5分钟后重新尝试连接.");
            Invoke(new VoidFunctionDelegate(delegate { _listTimer.Start(); }));
        }

        void _listTimer_Tick(object sender, EventArgs e)
        {
            ConnectServer();
        }

        void _listClient_OnConnectFail(NetworkLib.NetworkException error)
        {
            AppendText("无法连接到房间列表服务器.5分钟后重新尝试连接.");
            Invoke(new VoidFunctionDelegate(delegate { _listTimer.Start(); }));
        }

        private void CloseListClient()
        {
            if (_listClient != null)
            {
                _listClient.Stop();
                _listClient = null;
            }
        }

        private string GetServerIP()
        {
            return Properties.Settings.Default.ServerIP;
        }

        private void UpdateUserCount()
        {
            if (_listClient!=null) _listClient.RoomUpdateInfo();
        }
#endregion


    }

}