using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PokemonBattle.BattleRoom.Client;
using NetworkLib;
using System.Text.RegularExpressions;
using PokemonBattle.BattleNetwork;

namespace PokemonBattle.RoomClient
{
    public delegate Form BuildServerFormDelegate(string userName, BattleMode mode, List<BattleRule> rules);
    public delegate Form BuildClientFormDelegate(string serverAddress, byte position, string userName, BattleMode mode);
    public delegate Form BuildAgentFormDelegate(AgentBattleInfo battleInfo);
    public delegate Form BuildObserverFormDelegate(int identity, string serverAddress, byte playerPosition);
    public delegate Form BuildFourPlayerFormDelegate(int identity,string serverAddress,
        string userName, bool asHost, FourPlayerFormCallback callback);

    public delegate void FourPlayerFormCallback(int identity, byte playerPosition);
    public partial class RoomClientForm : Form
    {
        #region Varibles
        private User _myInfo;
        private string _serverIP;
        private RoomClient _client;
        private ChatManager _chatManager;
        private RoomBattleSetting _roomSetting;
        private string _version;
        private BroadcastController _bcController;
        #endregion

        #region Events
        public event BuildServerFormDelegate OnBuildBattleServerForm;
        private Form HandleBuildBattleServerFormEvent(string userName, BattleMode mode,List<BattleRule> rules)
        {
            if (OnBuildBattleServerForm != null) return OnBuildBattleServerForm(userName, mode, rules);
            return null;
        }
        public event BuildClientFormDelegate OnBuildBattleClientForm;
        private Form HandleBuildBattleClientFormEvent(string serverAddress, byte position, string userName, BattleMode mode)
        {
            if (OnBuildBattleClientForm != null) return OnBuildBattleClientForm(serverAddress, position, userName, mode);
            return null;
        }
        public event BuildAgentFormDelegate OnBuildBattleAgentForm;
        private Form HandleBuildBattleAgentFormEvent(AgentBattleInfo battleInfo)
        {
            if (OnBuildBattleAgentForm != null) return OnBuildBattleAgentForm(battleInfo);
            return null;
        }
        public event BuildObserverFormDelegate OnBuildBattleObserverForm;
        private Form HandleBuildBattleObserverFormEvent(int identity, string serverAddress, byte playerPosition)
        {
            if (OnBuildBattleObserverForm != null)
            {
                return OnBuildBattleObserverForm(identity,serverAddress,playerPosition);
            }
            return null;
        }
        public event BuildFourPlayerFormDelegate OnBuildFourPlayerForm;
        private Form HandleBuildFourPlayerFormEvent(int identity, string serverAddress, string userName, bool asHost,
            FourPlayerFormCallback callback)
        {
            if (OnBuildFourPlayerForm != null)
            {
                return OnBuildFourPlayerForm(identity, serverAddress, userName, asHost, callback);
            }
            return null;
        }
        #endregion

        public RoomClientForm(User userInfo, string serverAddress, string roomName, string version)
        {
            InitializeComponent();
            _myInfo = userInfo;
            _myInfo.State = UserState.Free;
            _serverIP = serverAddress;
            _version = version;
            Text = string.Format("房间窗口 - {0}", roomName);
        }

        #region EventHandler

        private void RoomUserForm_Load(object sender, EventArgs e)
        {
            RunClient();
            FormClosed += new FormClosedEventHandler(RoomUserForm_FormClosed);
            MessageText.KeyDown += new KeyEventHandler(MessageText_KeyDown);
            UserList.ListViewItemSorter = new ListViewSorter(0);
        }

        void RoomUserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseClient();
            if (_bcController != null) _bcController.Stop();
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
                if (CommandCheck.Checked)
                {
                    _client.RoomCommand(MessageText.Text);
                }
                else
                {
                    _client.Broadcast(string.Format("{0} : {1}", _myInfo.Name, MessageText.Text));
                    _bcController.Tick();
                }
                MessageText.Clear();
            }
        }

        void _bcController_OnCounterChanged()
        {
            if (!InvokeRequired)
            {
                if (_bcController.Counter >= 10)
                {
                    SendButton.Enabled = false;
                    BcCounterLabel.Text = "100%";
                }
                else
                {
                    SendButton.Enabled = true;
                    BcCounterLabel.Text = (_bcController.Counter * 10).ToString() + "%";
                }
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { _bcController_OnCounterChanged(); }));
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AwayMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (AwayMenuItem.Checked)
            {
                SetUserState(UserState.Away);
            }
            else
            {
                SetUserState(UserState.Free);
            }
        }

        private void UserInfoMenuItem_Click(object sender, EventArgs e)
        {
            UserInfoForm infoForm = new UserInfoForm(_myInfo);
            infoForm.Icon = Icon;
            if (infoForm.ShowDialog() == DialogResult.OK)
            {
                _client.UpdateInfo();
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (_client != null)
            {
                _client.RegistFourPlayer();
                CreateButton.Enabled = false;
                EnterButton.Enabled = false;
            }
        }

        private void FourPlayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_roomSetting.FourPlayerBan || _myInfo.State == UserState.Away) return;
            if (FourPlayerList.SelectedItems.Count > 0)
            {
                EnterButton.Enabled = true;
            }
            else
            {
                EnterButton.Enabled = false;
            }
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            if (FourPlayerList.SelectedItems.Count == 0)
            {
                EnterButton.Enabled = false;
                return;
            }
            ListViewItem item = FourPlayerList.SelectedItems[0];
            int identity = int.Parse(item.Name);
            Form form = HandleBuildFourPlayerFormEvent(identity, _serverIP, _myInfo.Name, false, Build4PBattleForm);
            if (form != null)
            {
                CreateButton.Enabled = false;
                EnterButton.Enabled = false;
                form.Show();
                form.FormClosed += FourPlayerForm_FormClosed;
            }
        }

        #endregion

        #region RoomClient
        private void RunClient()
        {
            _client = new RoomClient(_serverIP, _myInfo);

            _client.OnAddUserInfo += new UserDelegate(AddUserInfo);
            _client.OnUpdateUserInfo += new UserDelegate(UpdateUserInfo);
            _client.OnUpdateUserList += new UserListDelegate(UpdateUserList);
            _client.OnRemoveUserInfo += new IdentityDelegate(RemoveUserInfo);

            _client.OnConnectFail += new NetworkLib.NetworkErrorDelegate(_client_OnConnectFail);
            _client.OnLogonFailed += new MessageDelegate(_client_OnLogonFailed);
            _client.OnDisconnected += new NetworkEventDelegate(_client_OnDisconnected);
            _client.OnKicked += new VoidFunctionDelegate(_client_OnKicked);
            _client.OnLogoned += new UserDelegate(_client_OnLogoned);
            _client.OnSetting += new SettingDelegate(_client_OnSetting);

            _client.OnReceiveChat += new IdentityMessageDelegate(OnChat);
            _client.OnReceiveBroadcast += new MessageDelegate(_client_OnBroadcast);

            _client.OnReceiveChallenge += new ReceiveChallengeDelegate(_client_OnReceiveChallenge);
            _client.OnStartAgentBattle += new AgentBattleDelegate(BuildBattleAgentForm);
            _client.OnStartDirectBattle += new DirectBattleDelegate(BuildBattleClientForm);
            _client.OnBuildBattleServer += new BuildServerDelegate(BuildBattleServerForm);
            _client.OnObserveBattle += new ObserveBattleDelegate(_client_OnObserveBattle);

            _client.OnAdd4PRoom += new IdentityMessageDelegate(OnAdd4PRoom);
            _client.OnRemove4PRoom += new IdentityDelegate(OnRemove4PRoom);
            _client.OnUpdate4PRoom += new UpdateCountDelegate(OnUpdate4PRoom);
            _client.OnStart4PHost += new IdentityDelegate(OnStart4PHost);
            _client.OnAdd4PRoomList += new FourPlayerRoomListDelegate(_client_OnAdd4PRoomList);

            _client.Initialize();
            _client.RunThread();
        }

        private void _client_OnKicked()
        {
            if (!InvokeRequired)
            {
                MessageBox.Show("你被请出了房间.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { _client_OnKicked(); }));
            }
        }

        private void CloseClient()
        {
            if (_client != null)
            {
                _client.Stop();
                _client = null;
            }
        }

        private void _client_OnDisconnected()
        {
            if (!InvokeRequired)
            {
                MessageBox.Show("与房间服务器断开了连接.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { _client_OnDisconnected(); }));
            }
        }

        private void _client_OnLogoned(User info)
        {
            if (!InvokeRequired)
            {
                _myInfo.Identity = info.Identity;
                _myInfo.Address = info.Address;

                SendButton.Enabled = true;
                RoomMenuItem.Enabled = true;
                CreateButton.Enabled = true;

                _chatManager = new ChatManager();
                _chatManager.OnAddChat += new ChatDelegate(_chatManager_OnAddChat);
                _chatManager.OnRemoveChat += new ChatDelegate(_chatManager_OnRemoveChat);

                _bcController = new BroadcastController();
                _bcController.OnCounterChanged += new VoidFunctionDelegate(_bcController_OnCounterChanged);
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { _client_OnLogoned(info); }));
            }

        }

        private void _client_OnLogonFailed(string message)
        {
            if (!InvokeRequired)
            {
                MessageBox.Show(string.Format("无法登录房间服务器 : {0}.",message), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { _client_OnLogonFailed(message); }));
            }
        }

        private void _client_OnSetting(RoomBattleSetting setting)
        {
            _roomSetting = setting;
            if (!string.IsNullOrEmpty(setting.Version) && _version != setting.Version)
            {
                Invoke(new VoidFunctionDelegate(
                    delegate 
                    {
                        MessageBox.Show(string.Format("当前房间限制用户版本为{0}",setting.Version), "提示", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                ));
            }
        }

        private void _client_OnConnectFail(NetworkLib.NetworkException error)
        {
            _client_OnLogonFailed("连接错误.");
        }

        private void _client_OnBroadcast(string message)
        {
            AppendText(message);
        }

        private void OnStart4PHost(int identity)
        {
            if (!InvokeRequired)
            {
                Form form = HandleBuildFourPlayerFormEvent(identity, _serverIP, _myInfo.Name, true, Build4PBattleForm);
                if (form != null)
                {
                    form.Show();
                    form.FormClosed += new FormClosedEventHandler(FourPlayerForm_FormClosed);
                }
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { OnStart4PHost(identity); }));
            }
        }

        private void _client_OnAdd4PRoomList(FourPlayerRoomSequence rooms)
        {
            foreach (FourPlayerRoom room in rooms.Elements)
            {
                ListViewItem item = new ListViewItem(new string[] {room.Name,room.PlayerCount.ToString() });
                item.Name = room.Identity.ToString();
                Invoke(new VoidFunctionDelegate(delegate { FourPlayerList.Items.Add(item); }));
            }
        }

        public void UpdateCustomData(string name,string hash)
        {
            _myInfo.CustomDataHash = hash;
            _myInfo.CustomDataInfo = name;
            if (_client != null) _client.UpdateInfo();
        }

        public void BeginEditTeam()
        {
            if (!AwayMenuItem.Checked) AwayMenuItem.Checked = true;
            AwayMenuItem.Enabled = false;
        }

        public void EndEditTeam()
        {
            AwayMenuItem.Checked = false;
            AwayMenuItem.Enabled = true;
        }

        private void SetUserState(UserState state)
        {
            _myInfo.State = state;
            if (_client != null)
            {
                _client.UpdateInfo();
            }
        }

        #endregion

        #region 4PList

        private void FourPlayerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_roomSetting.FourPlayerBan)
            {
                CreateButton.Enabled = true;
                EnterButton.Enabled = true;
            }
        }

        private void OnUpdate4PRoom(int identity, byte count)
        {
            if (!InvokeRequired)
            {
                ListViewItem[] item = FourPlayerList.Items.Find(identity.ToString(), false);
                if (item.Length > 0)
                {
                    item[0].SubItems[1].Text = count.ToString();
                }
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { OnUpdate4PRoom(identity, count); }));
            }
        }

        private void OnRemove4PRoom(int identity)
        {
            Invoke(new VoidFunctionDelegate(delegate { FourPlayerList.Items.RemoveByKey(identity.ToString()); }));
        }

        private void OnAdd4PRoom(int identity, string message)
        {
            ListViewItem item = new ListViewItem(new string[] { message, "1" });
            item.Name = identity.ToString();
            Invoke(new VoidFunctionDelegate(delegate { FourPlayerList.Items.Add(item); }));
        }
        #endregion

        #region UserList

        private void RemoveUserInfo(int identity)
        {
            Invoke(new VoidFunctionDelegate(delegate { UserList.Items.RemoveByKey(identity.ToString()); }));
        }

        private void UpdateUserList(List<User> users)
        {
            if (!InvokeRequired)
            {
                UserList.BeginUpdate();
                UserList.Items.Clear();
                foreach (User userInfo in users)
                {
                    AddUserInfo(userInfo);
                }
                UserList.EndUpdate();
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { UpdateUserList(users); }));
            }
        }

        private void UpdateUserInfo(User userInfo)
        {
            if (!InvokeRequired)
            {
                ListViewItem[] item = UserList.Items.Find(userInfo.Identity.ToString(), false);
                if (item.Length > 0)
                {
                    item[0].SubItems[0].Text = userInfo.Name;
                    item[0].SubItems[1].Text = GetStateString(userInfo.State);
                    item[0].ImageIndex = userInfo.ImageKey;
                }
                if (userInfo.Identity == _myInfo.Identity) UpdateUserState();
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { UpdateUserInfo(userInfo); }));
            }
        }

        private void AddUserInfo(User userInfo)
        {
            ListViewItem item = new ListViewItem(new string[] { userInfo.Name, GetStateString(userInfo.State) });
            item.Name = userInfo.Identity.ToString();
            item.ImageIndex = userInfo.ImageKey;
            Invoke(new VoidFunctionDelegate(delegate { UserList.Items.Add(item); }));
        }

        #endregion

        #region Chat

        private void OnChat(int from, string message)
        {
            _chatManager.PassChatMessage(from, message);
        }

        private void ShowChat(int target)
        {
            ChatForm chat = _chatManager.GetChatForm(target);
            if (chat == null)
            {
                _chatManager.BuildChatForm(target, _myInfo.Name, _client);
                chat = _chatManager.GetChatForm(target);
            }
            chat.Icon = Icon;
            chat.MdiParent = MdiParent;
            chat.Show();
        }

        private void _chatManager_OnRemoveChat(int identity)
        {
            if (!InvokeRequired)
            {
                ChatsMenuItem.DropDownItems.RemoveByKey(identity.ToString());

                if (ChatsMenuItem.DropDownItems.Count == 0) ChatsMenuItem.Enabled = false;
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { _chatManager_OnRemoveChat(identity); }));
            }
        }

        private void _chatManager_OnAddChat(int identity)
        {
            if (!InvokeRequired)
            {
                User target = _client.GetUser(identity);
                if (target != null)
                {
                    ToolStripMenuItem chatMenu = new ToolStripMenuItem(target.Name);
                    chatMenu.Name = identity.ToString();
                    chatMenu.Tag = identity;
                    chatMenu.Click += new EventHandler(chatMenu_Click);
                    ChatsMenuItem.DropDownItems.Add(chatMenu);

                    if (ChatsMenuItem.Enabled == false) ChatsMenuItem.Enabled = true;
                }
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { _chatManager_OnAddChat(identity); }));
            }
        }

        private void chatMenu_Click(object sender, EventArgs e)
        {
            int target = (int)(sender as ToolStripMenuItem).Tag;
            ShowChat(target);
        }

        private void ChatToMenuItem_Click(object sender, EventArgs e)
        {
            int identity = SelectedUserIdentity;
            if (identity != -1 && identity !=_myInfo.Identity) ShowChat(identity);
        }

        #endregion

        #region Battle

        private void ChallengeMenuItem_Click(object sender, EventArgs e)
        {
            if (_roomSetting.SingleBan && _roomSetting.DoubleBan)
            {
                AppendText("提示 : 当前房间禁止单打与双打");
                return;
            }
            int identity = SelectedUserIdentity;
            if (identity != -1 && identity != _myInfo.Identity)
            {
                ChallengeForm challenge = _client.GetChallengeForm(identity);
                if (challenge != null)
                {
                    challenge.Icon = Icon;
                    challenge.MdiParent = MdiParent;
                    challenge.Show();
                    SetUserState(UserState.Challenging);
                }
            }
        }

        private void BuildBattleServerForm(BattleMode battleMode,BattleRuleSequence rules)
        {
            if (!InvokeRequired)
            {
                Form serverForm = HandleBuildBattleServerFormEvent(_myInfo.Name, battleMode, rules.Elements);
                if (serverForm != null)
                {
                    SetUserState(UserState.Battling);
                    serverForm.FormClosed += new FormClosedEventHandler(BattleFormClosed);
                    serverForm.Show();
                }
                else
                {
                    MessageBox.Show("please build battle server form");
                    SetUserState(UserState.Free);
                }
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { BuildBattleServerForm(battleMode, rules); }));
            }
        }

        private void BuildBattleClientForm(string serverAddress, BattleMode battleMode)
        {
            if (!InvokeRequired)
            {
                Form clientForm = HandleBuildBattleClientFormEvent(serverAddress, 2, _myInfo.Name, battleMode);
                if (clientForm != null)
                {
                    SetUserState(UserState.Battling);
                    clientForm.FormClosed += new FormClosedEventHandler(BattleFormClosed);
                    clientForm.Show();
                }
                else
                {
                    MessageBox.Show("please build battle client form");
                    SetUserState(UserState.Free);
                }
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { BuildBattleClientForm(serverAddress, battleMode); }));
            }
        }

        private void BuildBattleAgentForm(int identity, byte myPosition, BattleMode battleMode)
        {
            if (!InvokeRequired)
            {
                AgentBattleInfo battleInfo = new AgentBattleInfo();
                battleInfo.AgentID = identity;
                battleInfo.BattleMode = battleMode;
                battleInfo.Position = myPosition;
                battleInfo.ServerAddress = _serverIP;
                battleInfo.UserName = _myInfo.Name;
                battleInfo.MoveInterval = _roomSetting.MoveInterval;
                Form agentForm = HandleBuildBattleAgentFormEvent(battleInfo);
                if (agentForm != null)
                {
                    SetUserState(UserState.Battling);
                    agentForm.FormClosed += new FormClosedEventHandler(BattleFormClosed);
                    agentForm.Show();
                }
                else
                {
                    MessageBox.Show("please build battle agent form");
                    SetUserState(UserState.Free);
                }
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { BuildBattleAgentForm(identity, myPosition, battleMode); }));
            }
        }

        private void BuildBattleObserverForm(int identity, string serverAddress, byte position)
        {
            if (!InvokeRequired)
            {
                Form observerForm = HandleBuildBattleObserverFormEvent(identity, serverAddress, position);
                if (observerForm != null) observerForm.Show();
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { BuildBattleObserverForm(identity, serverAddress, position); }));
            }
        }

        private void Build4PBattleForm(int identity, byte position)
        {
            BuildBattleAgentForm(identity, position, BattleMode.Double_4P);
            _client.StartFourPlayerBattle(identity, position);
        }

        private void BattleFormClosed(object sender, FormClosedEventArgs e)
        {
            SetUserState(UserState.Free);
        }

        private void _client_OnReceiveChallenge(ChallengeForm challenge)
        {
            Invoke(new VoidFunctionDelegate(
                delegate
                {
                    challenge.Icon = Icon;
                    challenge.MdiParent = MdiParent;
                    challenge.Show();
                    challenge.Activate();
                }
                ));
        }

        private void ObserveMenuItem_Click(object sender, EventArgs e)
        {
            if (_client == null) return;
            int identity = SelectedUserIdentity;
            User user = _client.GetUser(identity);
            if (user != null && user.State == UserState.Battling)
            {
                _client.ObserveBattle(identity);
            }
        }

        private void _client_OnObserveBattle(ObserveInfo info)
        {
            if (info.BattleIdentity != -1) info.BattleAddress = _serverIP;
            BuildBattleObserverForm(info.BattleIdentity, info.BattleAddress, info.Position);
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
                DisplayText.Text += String.Format("{0}", text);

                DisplayText.SelectionStart = DisplayText.Text.Length - 1;
                DisplayText.ScrollToCaret();
            }
            else
            {
                Invoke(new VoidFunctionDelegate(delegate { AppendText(text); }));
            }
        }

        private void UpdateUserState()
        {
            switch (_myInfo.State)
            {
                case UserState.Away:
                    ChallengeMenuItem.Enabled = false;
                    CreateButton.Enabled = false;
                    EnterButton.Enabled = false;
                    break;
                case UserState.Battling:
                    AwayMenuItem.Enabled = false;
                    ChallengeMenuItem.Enabled = false;
                    CreateButton.Enabled = false;
                    EnterButton.Enabled = false;
                    break;
                case UserState.Challenging:
                    AwayMenuItem.Enabled = false;
                    ChallengeMenuItem.Enabled = false;
                    CreateButton.Enabled = false;
                    EnterButton.Enabled = false;
                    break;
                case UserState.Free:
                    AwayMenuItem.Enabled = true;
                    ChallengeMenuItem.Enabled = true;
                    if (!_roomSetting.FourPlayerBan)
                    {
                        CreateButton.Enabled = true;
                    }
                    break;
            }
        }

        private int SelectedUserIdentity
        {
            get
            {
                if (UserList.SelectedItems.Count > 0)
                {
                    int identity = int.Parse(UserList.SelectedItems[0].Name);
                    return identity;
                }
                return -1;
            }
        }

        private string GetStateString(UserState state)
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

        public static bool IsEmptyString(string str)
        {
            return Regex.IsMatch(str, "^\\s*$");
        }

    }
}
