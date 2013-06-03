using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using NetworkLib.Tcp;
using NetworkLib.Utilities;
using PokemonBattle.BattleRoom.Server;
using PokemonBattle.BattleNetwork;
using System.Threading;

namespace PokemonBattle.RoomServer
{
    public delegate bool VerifyUserDelegate(User userInfo,out string failMessage);
    public delegate void UserDelegate(User userInfo);
    public delegate void RemoveUserDelegate(int identity);
    public delegate void MessageDelegate(string message);
    public delegate void CommandDelegate(int identity,string command);
    public class RoomServer :NetworkServer,IProtocolInterpreter ,IRoomServerService
    {
        private Dictionary<int, User> _users = new Dictionary<int, User>();
        private Dictionary<int, User> _removeUsers = new Dictionary<int, User>();

        private BattleAgentServer _battleAgent = new BattleAgentServer();
        private FourPlayerAgentServer _fourPlayerAgent = new FourPlayerAgentServer();

        private RoomBattleSetting _setting;
        #region Events
        public event VerifyUserDelegate OnVerifyUser;
        private bool HandleOnVerifyUserEvent(User info, out string failMessage)
        {
            if (OnVerifyUser != null) return OnVerifyUser(info, out failMessage);
            failMessage = "";
            return true;
        }

        public event UserDelegate OnAddUserInfo;
        private void HandleOnAddUserEvent(User info)
        {
            if (OnAddUserInfo != null) OnAddUserInfo(info);
        }

        public event UserDelegate OnUpdateUserInfo;
        private void HandleOnUpdateUserEvent(User info)
        {
            if (OnUpdateUserInfo != null) OnUpdateUserInfo(info);
        }

        public event RemoveUserDelegate OnRemoveUserInfo;
        private void HandleOnRemoveUserEvent(int identity)
        {
            if (OnRemoveUserInfo != null) OnRemoveUserInfo(identity);
        }

        public event MessageDelegate OnGetMessage;
        private void HandleOnGetMessageEvent(string message)
        {
            if (OnGetMessage != null) OnGetMessage(message);
        }

        public event CommandDelegate OnRoomCommand;
        #endregion

        public RoomServer(RoomBattleSetting setting)
        {
            _setting = setting;

            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = 10072;
            strategy.Sync = true;

            NetworkStrategy = strategy;
            _interpreter = this;
            UpdateInterval = 100;
            OnLogicUpdate += LogicUpdate;

            Logger.LogInfo("Room server start to work.");
            OnClientDisconnected += new SessionDisconnectedDelegate(RoomServer_OnClientDisconnected);
            _fourPlayerAgent.OnRemoveRoom += new RemoveDelegate(_fourPlayerAgent_OnRemoveRoom);
            _fourPlayerAgent.OnUpdateRoom += new UpdateCountDelegate(_fourPlayerAgent_OnUpdateRoom);
            _fourPlayerAgent.OnAddBattle += new IntFunctionDelegate(_fourPlayerAgent_OnAddBattle);
        }

        protected override bool InitializeImpl()
        {
            if (base.InitializeImpl())
            {
                if (!_battleAgent.Initialize())
                {
                    return false;
                }
                _battleAgent.RunThread();
                if (!_fourPlayerAgent.Initialize())
                {
                    return false;
                }
                _fourPlayerAgent.RunThread();
                return true;
            }
            return false;
        }

        protected override void StopImpl()
        {
            if (_battleAgent != null)
            {
                _battleAgent.Stop();
                _battleAgent = null;
            }
            if (_fourPlayerAgent != null)
            {
                _fourPlayerAgent.Stop();
                _fourPlayerAgent = null;
            }
            base.StopImpl();
        }

        protected override ClientSession CreateClientSession(int sessionID, IReactor reactor)
        {
            return new RoomClientSession(sessionID, reactor, Buffered);
        }

        void RoomServer_OnClientDisconnected(ClientSession client)
        {
            User user = GetUser(client.SessionID);
            if (user != null)
            {
                _removeUsers[client.SessionID] = user;
            }
        }

        private void LogicUpdate()
        {
            Dictionary<int, User> removeList = new Dictionary<int, User>();
            _removeUsers = Interlocked.Exchange(ref removeList, _removeUsers);

            foreach (int key in removeList.Keys)
            {
                _users.Remove(key);
                BroadCast(RoomServerHelper.RemoveUser(key));
                HandleOnRemoveUserEvent(key);
            }
        }


        public void BroadcastMessage(string message)
        {
            BroadCast(RoomServerHelper.ReceiveBroadcastMessage(message));
            HandleOnGetMessageEvent(message);
        }

        public void SendMessage(int sessionID, string message)
        {
            Send(sessionID, RoomServerHelper.ReceiveBroadcastMessage(message));
        }

        public void OnReceiveRoomCommand(int sessionID, string message)
        {
            if (OnRoomCommand != null) OnRoomCommand(sessionID, message);
        }

        public void KickUser(int identity)
        {
            if (_users.ContainsKey(identity))
            {
                Send(identity, RoomServerHelper.BeKicked());
                Disconnect(identity);

                Logger.LogInfo("User was kicked , ID : {0}", identity);
            }
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

        public bool ExistsUser(Predicate<User> predicate)
        {
            List<User> userList = new List<User>(_users.Values);
            return userList.Exists(predicate);
        }

        public void UpdateSetting()
        {
            BroadCast(RoomServerHelper.UpdateRoomSetting(_setting));
        }

        private int AddAgentBattle(ChallengeInfo info)
        {
            if (_battleAgent != null)
            {
                return _battleAgent.AddBattle(info.BattleMode, info.Rules);
            }
            return -1;
        }

        #region 4P

        void _fourPlayerAgent_OnUpdateRoom(int identity, byte count)
        {
            BroadCast(RoomServerHelper.UpdateFourPlayerRoom(identity, count));
        }

        void _fourPlayerAgent_OnRemoveRoom(int identity)
        {
            BroadCast(RoomServerHelper.RemoveFourPlayerRoom(identity));
        }

        int _fourPlayerAgent_OnAddBattle()
        {
            if (_battleAgent != null) return _battleAgent.AddBattle(BattleMode.Double_4P, new BattleRuleSequence());
            return -1;
        }

        #endregion

        #region IRoomServerService 成员

        public void OnAcceptChallenge(int sessionID, int targetIdentity)
        {
            if (!_users.ContainsKey(targetIdentity)) return;
            Send(targetIdentity, RoomServerHelper.AcceptChallenge(sessionID));
        }

        public void OnRefuseChallenge(int sessionID, int targetIdentity)
        {
            if (!_users.ContainsKey(targetIdentity)) return;
            Send(targetIdentity, RoomServerHelper.RefuseChallenge(sessionID));
        }

        public void OnChallenge(int sessionID, int targetIdentity, ChallengeInfo info)
        {
            if (!_users.ContainsKey(targetIdentity)) return;
            Send(targetIdentity, RoomServerHelper.Challenge(sessionID, info));
        }

        public void OnCancelChallenge(int sessionID, int targetIdentity)
        {
            if (!_users.ContainsKey(targetIdentity)) return;
            Send(targetIdentity, RoomServerHelper.CancelChallenge(sessionID));
        }


        public void OnStartBattle(int sessionID, int with, ChallengeInfo info)
        {
            if (!_users.ContainsKey(with)) return;
            ClientSession client = GetClient(sessionID);
            ClientSession client2 = GetClient(with);
            User user = GetUser(sessionID);
            if (user == null) return;
            if (client == null || client2 == null) return;

            ObserveInfo obInfo1 = (client as RoomClientSession).ObserveInfo;
            obInfo1.Position = 1;
            ObserveInfo obInfo2 = (client2 as RoomClientSession).ObserveInfo;
            obInfo2.Position = 2;
            if (info.LinkMode == BattleLinkMode.Direct)
            {
                Send(with, RoomServerHelper.DirectBattle(sessionID, info.BattleMode));

                obInfo1.BattleAddress = user.Address;
                obInfo1.BattleIdentity = -1;

                obInfo2.BattleAddress = user.Address;
                obInfo2.BattleIdentity = -1;
            }
            else
            {
                int agentIdentity = AddAgentBattle(info);
                Send(sessionID, RoomServerHelper.AgentBattle(agentIdentity, 1, info.BattleMode));
                Send(with, RoomServerHelper.AgentBattle(agentIdentity, 2, info.BattleMode));
                obInfo1.BattleAddress = string.Empty;
                obInfo1.BattleIdentity = agentIdentity;
                obInfo2.BattleAddress = string.Empty;
                obInfo2.BattleIdentity = agentIdentity;
            }
        }

        public void OnStartFourPlayerBattle(int sessionID, int battleIdentity, byte position)
        {
            ClientSession client = GetClient(sessionID);
            if (client != null)
            {
                ObserveInfo info = (client as RoomClientSession).ObserveInfo;
                info.BattleAddress = string.Empty;
                info.BattleIdentity = battleIdentity;
                info.Position = position;
            }
        }

        public void OnGetObserveInfo(int sessionID, int identity)
        {
            ClientSession client = GetClient(identity);
            User user = GetUser(identity);
            if (client != null && user != null && user.State == UserState.Battling)
            {
                Send(sessionID, RoomServerHelper.ReceiveObserveInfo((client as RoomClientSession).ObserveInfo));
            }
        }

        public void OnRegistFourPlayer(int sessionID)
        {
            string userName = _users[sessionID].Name;
            int agentID = _fourPlayerAgent.AddAgent(userName);
            Send(sessionID, RoomServerHelper.RegistFourPlayerSuccess(agentID));
            BroadCast(RoomServerHelper.AddFourPlayerRoom(agentID, userName));
        }


        public void OnUserLogon(int sessionID, User info)
        {
            string message;
            info.Address = GetClient(sessionID).ClientAddress.ToString();
            if (HandleOnVerifyUserEvent(info, out message))
            {
                info.Identity = sessionID;
                _users[sessionID] = info;

                Send(sessionID, RoomServerHelper.LogonSuccess(info));
                Send(sessionID, RoomServerHelper.UpdateRoomSetting(_setting));

                BroadCast(RoomServerHelper.AddNewUser(info));

                UserSequence users = new UserSequence();
                users.Elements.AddRange(_users.Values);
                Send(sessionID, RoomServerHelper.AddUserList(users));

                Send(sessionID, RoomServerHelper.AddFourPlayerRoomList(Get4PRooms()));

                OnAddUserInfo(info);
                Logger.LogInfo("New user logoned, ID : {0} , Name : {1} , Address : {2}", sessionID, info.Name, info.Address);
            }
            else
            {
                Send(sessionID, RoomServerHelper.LogonFail(message));
                Disconnect(sessionID);

                Logger.LogInfo("User logon failed, ID : {0} , Name : {1} , Address : {2}", sessionID, info.Name, info.Address);
            }
        }

        public void OnUserLogout(int sessionID)
        {
            Disconnect(sessionID);

            Logger.LogInfo("User logouted , ID : {0}", sessionID);
        }

        public void OnUpdateUser(int sessionID, User info)
        {
            _users[sessionID] = info;
            BroadCast(RoomServerHelper.UpdateUser(info));

            HandleOnUpdateUserEvent(info);
            Logger.LogInfo("User updated, ID : {0} , Name : {1}", sessionID, info.Name);
        }


        public void OnReceiveBroadcastMessage(int sessionID, string message)
        {
            BroadCast(RoomServerHelper.ReceiveBroadcastMessage(message));
            OnGetMessage(message);
        }

        public void OnReceiveChatMessage(int sessionID, int to, string message)
        {
            Send(to, RoomServerHelper.ReceiveChatMessage(sessionID,message));
        }

        #endregion

        private FourPlayerRoomSequence Get4PRooms()
        {

            FourPlayerRoomSequence rooms = new FourPlayerRoomSequence();
            List<FourPlayerAgent> agents = _fourPlayerAgent.GetAgents();
            foreach (FourPlayerAgent agent in agents)
            {
                FourPlayerRoom room = new FourPlayerRoom();
                room.Name = agent.HostName;
                room.Identity = agent.AgentID;
                room.PlayerCount = agent.GetPlayerCount();
                rooms.Elements.Add(room);
            }
            return rooms;
        }

        #region IProtocolInterpreter 成员

        public bool InterpretMessage(int sessionID, NetworkLib.Utilities.ByteArray byteArray)
        {
            return RoomServerHelper.InterpretMessage(sessionID, byteArray, this);
        }

        #endregion

    }
}
