using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using NetworkLib.Tcp;
using PokemonBattle.BattleRoom.Client;
using System.Net;
using NetworkLib.Utilities;
using PokemonBattle.BattleNetwork;
using System.Threading;

namespace PokemonBattle.RoomClient
{
    public delegate void UserListDelegate(List<User> users);
    public delegate void UserDelegate(User userInfo);
    public delegate void IdentityDelegate(int identity);
    public delegate void MessageDelegate(string message);
    public delegate void IdentityMessageDelegate(int identity,string message);
    public delegate void ReceiveChallengeDelegate(ChallengeForm challenge);
    public delegate void AgentBattleDelegate(int identity,byte playerPosition, BattleMode mode);
    public delegate void BuildServerDelegate(BattleMode mode,BattleRuleSequence rules);
    public delegate void DirectBattleDelegate(string serverAddress, BattleMode mode);
    public delegate void ObserveBattleDelegate(ObserveInfo info);
    public delegate void FourPlayerRoomListDelegate(FourPlayerRoomSequence rooms);
    public delegate void SettingDelegate(RoomBattleSetting setting);
    
    public class RoomClient : NetworkClient, IProtocolInterpreter,IRoomClientService
    {
        #region Varibles
        private Dictionary<int, User> _users = new Dictionary<int, User>();
        private Dictionary<int, User> _removeUsers = new Dictionary<int, User>();

        private User _userInfo;
        public User UserInfo
        {
            get { return _userInfo; }
        }

        private ChallengeManager _challengeManager;

        #endregion

        #region Events

        public event UserDelegate OnUpdateUserInfo;
        private void HandleOnUpdateUserInfoEvent(User userInfo)
        {
            if (OnUpdateUserInfo != null)
            {
                OnUpdateUserInfo(userInfo);
            }
        }

        public event UserDelegate OnAddUserInfo;
        private void HandleAddUserInfoEvent(User userInfo)
        {
            if (OnAddUserInfo != null)
            {
                OnAddUserInfo(userInfo);
            }
        }

        public event IdentityDelegate OnRemoveUserInfo;
        private void HandleRemoveUserInfoEvent(int identity)
        {
            if (OnRemoveUserInfo != null)
            {
                OnRemoveUserInfo(identity);
            }
        }

        public event UserListDelegate OnUpdateUserList;
        private void HandleUpdateUserListEvent(List<User> users)
        {
            if (OnUpdateUserList != null)
            {
                OnUpdateUserList(users);
            }
        }

        public event VoidFunctionDelegate OnKicked;
        public event UserDelegate OnLogoned;
        public event SettingDelegate OnSetting;
        public event MessageDelegate OnLogonFailed;
        public event MessageDelegate OnReceiveBroadcast;

        public event IdentityMessageDelegate OnReceiveChat;
        public event ReceiveChallengeDelegate OnReceiveChallenge;

        public event AgentBattleDelegate OnStartAgentBattle;
        public event DirectBattleDelegate OnStartDirectBattle;
        public event ObserveBattleDelegate OnObserveBattle;

        public event IdentityDelegate OnStart4PHost;
        public event IdentityMessageDelegate OnAdd4PRoom;
        public event IdentityDelegate OnRemove4PRoom;
        public event UpdateCountDelegate OnUpdate4PRoom;
        public event FourPlayerRoomListDelegate OnAdd4PRoomList;

        public event BuildServerDelegate OnBuildBattleServer;
        private void HandleBuildBattleServerEvent(BattleMode mode, BattleRuleSequence rules)
        {
            if (OnBuildBattleServer != null) OnBuildBattleServer(mode, rules);
        }
        #endregion

        public RoomClient(string serverIP,User info)
        {
            _userInfo = info;

            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = 10072;
            strategy.ServerIP = serverIP;
            strategy.Sync = true;
            NetworkStrategy = strategy;

            _interpreter = this;
            UpdateInterval = 200;

            OnLogicUpdate += new VoidFunctionDelegate(RoomClient_OnLogicUpdate);
            OnConnected += new NetworkEventDelegate(RoomClient_OnConnected);
            _challengeManager = new ChallengeManager(this);
        }

        void RoomClient_OnConnected()
        {
            Send(RoomClientHelper.UserLogon(_userInfo));
        }

        void RoomClient_OnLogicUpdate()
        {
            Dictionary<int, User> removeList = new Dictionary<int, User>();
            _removeUsers = Interlocked.Exchange(ref removeList, _removeUsers);

            foreach (int key in removeList.Keys)
            {
                _users.Remove(key);
                HandleRemoveUserInfoEvent(key);
            }
        }

        public void Broadcast(string message)
        {
            Send(RoomClientHelper.ReceiveBroadcastMessage(message));
        }

        public bool Chat(int target, string message)
        {
            if (_users.ContainsKey(target))
            {
                Send(RoomClientHelper.ReceiveChatMessage(target, message));
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RoomCommand(string message)
        {
            Send(RoomClientHelper.ReceiveRoomCommand(message));
        }

        public void UpdateInfo()
        {
            Send(RoomClientHelper.UpdateUser(_userInfo));
        }

        public ChallengeForm GetChallengeForm(int target)
        {
            User targetInfo = GetUser(target);
            if (targetInfo == null) return null;
            if (targetInfo.State != UserState.Free)
            {
                OnReceiveBroadcast("提示 : 该用户不是空闲状态,无法挑战.");
                return null;
            }
            if (targetInfo.CustomDataHash != _userInfo.CustomDataHash)
            {
                string customName = targetInfo.CustomDataInfo;
                if (string.IsNullOrEmpty(customName)) customName = "空";
                OnReceiveBroadcast(string.Format("提示 : 该用户的自定义数据为{0},与你不同,无法挑战.", customName));
                return null;
            }

            return _challengeManager.Challenge(targetInfo);
        }

        public void Challenge(int target,ChallengeInfo info)
        {
            Send(RoomClientHelper.Challenge(target, info));
        }

        public void AcceptChallenge(int target)
        {
            Send(RoomClientHelper.AcceptChallenge(target));
        }

        public void RefuseChallenge(int target)
        {
            Send(RoomClientHelper.RefuseChallenge(target));
        }

        public void CancelChallenge(int target)
        {
            Send(RoomClientHelper.CancelChallenge(target));
            _userInfo.State = UserState.Free;
            UpdateInfo();
        }

        public void StartBattle(int with, ChallengeInfo info)
        {
            if (info.LinkMode == BattleLinkMode.Direct)
            {
                HandleBuildBattleServerEvent(info.BattleMode, info.Rules);
            }
            Send(RoomClientHelper.StartBattle(with, info));
        }

        public void StartFourPlayerBattle(int identity, byte position)
        {
            Send(RoomClientHelper.StartFourPlayerBattle(identity, position));
        }

        public void ObserveBattle(int target)
        {
            Send(RoomClientHelper.GetObserveInfo(target));
        }

        public void RegistFourPlayer()
        {
            Send(RoomClientHelper.RegistFourPlayer());
        }

        public User GetUser(int identity)
        {
            User user;
            bool contains = _users.TryGetValue(identity, out user);
            if (contains)
            {
                return user;
            }
            return null;
        }

        protected override void StopImpl()
        {
            Send(RoomClientHelper.UserLogout());
            base.StopImpl();
        }

        #region IRoomClientService 成员

        public void OnAcceptChallenge(int from)
        {
            _challengeManager.ChallengeAccepted();
        }

        public void OnRefuseChallenge(int from)
        {
            _challengeManager.ChallengeRefused();
            _userInfo.State = UserState.Free;
            UpdateInfo();
        }

        public void OnChallenge(int from, ChallengeInfo info)
        {
            User user = GetUser(from);
            if (user == null) return;
            if (_userInfo.State == UserState.Free)
            {
                if (OnReceiveChallenge != null)
                {
                    OnReceiveChallenge(_challengeManager.ReceiveChallenge(user, info));
                }
            }
            else
            {
                RefuseChallenge(from);
            }
        }

        public void OnCancelChallenge(int from)
        {
            _challengeManager.ChallengeCanceled(from);
        }


        public void OnDirectBattle(int server, BattleMode battleMode)
        {
            User user = GetUser(server);
            if (user != null && OnStartDirectBattle != null)
            {
                OnStartDirectBattle(user.Address, battleMode);
            }
        }

        public void OnAgentBattle(int identity, byte playerPosition, BattleMode battleMode)
        {
            if (OnStartAgentBattle != null) OnStartAgentBattle(identity, playerPosition, battleMode);
        }

        public void OnReceiveObserveInfo(ObserveInfo info)
        {
            if (OnObserveBattle != null) OnObserveBattle(info);
        }


        public void OnRegistFourPlayerSuccess(int identity)
        {
            if (OnStart4PHost != null) OnStart4PHost(identity);
        }

        public void OnAddFourPlayerRoom(int identity, string host)
        {
            if (OnAdd4PRoom != null) OnAdd4PRoom(identity, host);
        }

        public void OnRemoveFourPlayerRoom(int identity)
        {
            if (OnRemove4PRoom != null) OnRemove4PRoom(identity);
        }

        public void OnUpdateFourPlayerRoom(int identity, byte userCount)
        {
            if (OnUpdate4PRoom != null) OnUpdate4PRoom(identity, userCount);
        }

        public void OnAddFourPlayerRoomList(FourPlayerRoomSequence rooms)
        {
            if (OnAdd4PRoomList != null) OnAdd4PRoomList(rooms);
        }

        public void OnReceiveBroadcastMessage(string message)
        {
            if (OnReceiveBroadcast != null) OnReceiveBroadcast(message);
        }

        public void OnReceiveChatMessage(int from, string message)
        {
            if (OnReceiveChat != null) OnReceiveChat(from, message);
        }


        public void OnLogonSuccess(User info)
        {
            if (OnLogoned != null) OnLogoned(info);
        }

        public void OnLogonFail(string message)
        {
            if (OnLogonFailed != null) OnLogonFailed(message);
        }

        public void OnBeKicked()
        {
            if (OnKicked != null) OnKicked();
        }

        public void OnUpdateRoomSetting(RoomBattleSetting setting)
        {
            _challengeManager.SetSetting(setting);
            if (OnSetting != null) OnSetting(setting);
        }


        public void OnUpdateUser(User userInfo)
        {
            _users[userInfo.Identity] = userInfo;
            HandleOnUpdateUserInfoEvent(userInfo);
            Logger.LogInfo("Update user info, ID : {0}", userInfo.Identity);
        }

        public void OnAddNewUser(User userInfo)
        {
            _users[userInfo.Identity] = userInfo;
            HandleAddUserInfoEvent(userInfo);
            Logger.LogInfo("New user info, ID : {0}, Name : {1}", userInfo.Identity, userInfo.Name);
        }

        public void OnRemoveUser(int identity)
        {
            User user = GetUser(identity);
            if (user != null)
            {
                _removeUsers[identity] = user;
                if (_challengeManager.UserLogout(identity))
                {
                    _userInfo.State = UserState.Free;
                    UpdateInfo();
                }
                Logger.LogInfo("Remove user , ID : {0} ", identity);
            }
        }

        public void OnAddUserList(UserSequence users)
        {
            foreach (User info in users.Elements)
            {
                _users[info.Identity] = info;
            }
            HandleUpdateUserListEvent(users.Elements);
            Logger.LogInfo("Get user list , Length : {0} ", users.Elements.Count);
        }

        #endregion

        #region IProtocolInterpreter 成员

        public bool InterpretMessage(int sessionID, NetworkLib.Utilities.ByteArray byteArray)
        {
            return RoomClientHelper.InterpretMessage(sessionID, byteArray, this);
        }

        #endregion
    }
}
