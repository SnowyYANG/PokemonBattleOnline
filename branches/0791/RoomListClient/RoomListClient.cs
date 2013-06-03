using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using NetworkLib.Utilities;
using NetworkLib.Tcp;
using PokemonBattle.RoomList.Client;
using System.Net;
using System.Threading;

namespace PokemonBattle.RoomListClient
{
    public enum ClientType
    {
        Room,
        User
    }
    public delegate void RoomListDelegate(List<Room> rooms);
    public delegate void RoomDelegate(Room roomInfo);
    public delegate void RemoveRoomDelegate(int identity);
    public delegate void LogonFailDelegate(string message);

    public class RoomListClient : NetworkClient, IProtocolInterpreter, IRoomListClientService
    {
        #region Varibles

        private ClientType _clientType;

        #region Room
        private Room _room;
        public Room Room
        {
            get
            {
                if (_clientType == ClientType.User) return null;
                return _room;
            }
            set
            {
                if (_clientType == ClientType.User) return;
                _room = value;
            }
        }

        public event VoidFunctionDelegate OnRoomLogoned;
        private void HandleOnLogonedEvent()
        {
            if (OnRoomLogoned != null)
            {
                OnRoomLogoned();
            }
        }

        public event LogonFailDelegate OnRoomLogonFailed;
        private void HandleOnLogonFailedEvent(string message)
        {
            if (OnRoomLogonFailed != null)
            {
                OnRoomLogonFailed(message);
            }
        }
        #endregion

        #region User
        private Dictionary<int, Room> _rooms = new Dictionary<int, Room>();
        private List<int> _removeRooms = new List<int>();

        public event RoomListDelegate OnUserAddRooms;
        private void HandleOnAddRoomsEvent(List<Room> rooms)
        {
            if (OnUserAddRooms != null)
            {
                OnUserAddRooms(rooms);
            }
        }

        public event RoomDelegate OnUpdateRoomInfo;
        private void HandleOnUpdateRoomInfoEvent(Room roomInfo)
        {
            if (OnUpdateRoomInfo != null)
            {
                OnUpdateRoomInfo(roomInfo);
            }
        }
        public event RoomDelegate OnAddRoomInfo;
        private void HandleOnAddRoomInfoEvent(Room roomInfo)
        {
            if (OnAddRoomInfo != null)
            {
                OnAddRoomInfo(roomInfo);
            }
        }
        public event RemoveRoomDelegate OnRemoveRoomInfo;
        private void HandleOnRemoveRoomInfoEvent(int identity)
        {
            if (OnRemoveRoomInfo != null)
            {
                OnRemoveRoomInfo(identity);
            }
        }
        #endregion

        #endregion

        public RoomListClient(string serverIP,Room roomInfo)
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Sync = true;
            strategy.ServerIP = serverIP;
            strategy.Port = 10069;
            NetworkStrategy = strategy;

            _clientType = ClientType.Room;
            _interpreter = this;
            _room = roomInfo;

            OnConnected += RoomConnected;
        }

        public RoomListClient(string serverIP)
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Sync = true;
            strategy.ServerIP = serverIP;
            strategy.Port = 10070;
            NetworkStrategy = strategy;

            _clientType = ClientType.User;
            _interpreter = this;

            UpdateInterval = 200;
            OnLogicUpdate += UserLogicUpdate;
            OnConnected += UserConnected;
        }

        private void UserConnected()
        {
            Send(RoomListClientHelper.UserLogon());
        }
        private void UserLogicUpdate()
        {
            List<int> removeList = new List<int>();
            _removeRooms = Interlocked.Exchange(ref removeList, _removeRooms);
            foreach (int key in removeList)
            {
                _rooms.Remove(key);
                HandleOnRemoveRoomInfoEvent(key);
            }

        }
        public Room GetRoom(int identity)
        {
            Room room;
            bool contains = _rooms.TryGetValue(identity, out room);
            if (contains)
            {
                return room;
            }
            return null;
        }

        private void RoomConnected()
        {
            Send(RoomListClientHelper.RoomLogon(_room));
        }

        public void RoomUpdateInfo()
        {
            if (_clientType == ClientType.Room)
            {
                Send(RoomListClientHelper.UpdateRoomInfo(_room));
            }
        }

        protected override void StopImpl()
        {
            if (_clientType == ClientType.User)
            {
                Send(RoomListClientHelper.UserLogout());
            }
            else if (_clientType == ClientType.Room)
            {
                Send(RoomListClientHelper.RoomLogout());
            }
            base.StopImpl();
        }

        #region IProtocolInterpreter 成员

        public bool InterpretMessage(int sessionID, ByteArray byteArray)
        {
            return RoomListClientHelper.InterpretMessage(sessionID, byteArray, this);
        }

        #endregion

        #region IRoomListClientService 成员

        #region Room
        public void OnLogonSuccess()
        {
            if (_clientType == ClientType.Room)
            {
                Logger.LogInfo("Room logon succeeded");
                HandleOnLogonedEvent();
            }
        }

        public void OnLogonFail(string message)
        {
            if (_clientType == ClientType.Room)
            {
                Logger.LogInfo("Room logon failed");
                HandleOnLogonFailedEvent(message);
            }
        }
        #endregion

        #region User
        public void OnUpdateRoom(Room roomInfo)
        {
            if (_clientType == ClientType.User)
            {
                _rooms[roomInfo.Identity] = roomInfo;
                HandleOnUpdateRoomInfoEvent(roomInfo);
                Logger.LogInfo("Update room info , ID : {0} , Name : {1}", roomInfo.Identity, roomInfo.Name);
            }
        }

        public void OnRemoveRoom(int identity)
        {
            if (_clientType == ClientType.User)
            {
                _removeRooms.Add(identity);
                Logger.LogInfo("Remove room , ID : {0} ", identity);
            }
        }

        public void OnAddNewRoom(Room roomInfo)
        {
            if (_clientType == ClientType.User)
            {
                _rooms[roomInfo.Identity] = roomInfo;
                HandleOnAddRoomInfoEvent(roomInfo);
                Logger.LogInfo("New room info , ID : {0} , Name : {1}", roomInfo.Identity, roomInfo.Name);
            }
        }

        public void OnAddRoomList(RoomSequence rooms)
        {
            if (_clientType == ClientType.User)
            {
                foreach (Room room in rooms.Elements)
                {
                    _rooms[room.Identity] = room;
                }
                HandleOnAddRoomsEvent(rooms.Elements);
                Logger.LogInfo("Get room list , Length : {0} ", rooms.Elements.Count);
            }
        }
        #endregion

        #endregion

    }
}
