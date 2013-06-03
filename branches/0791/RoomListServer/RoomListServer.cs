using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using NetworkLib.Utilities;
using NetworkLib.Tcp;
using PokemonBattle.RoomList.Server;
using System.Threading;

namespace PokemonBattle.RoomListServer
{
    public class RoomListServer : IProtocolInterpreter, IRoomListServerService
    {
        public delegate bool VerifyRoomDelegate(Room roomInfo,out string message);

        #region Varibles

        private NetworkServer _roomServer;
        private NetworkServer _userServer;

        private Dictionary<int, Room> _rooms = new Dictionary<int, Room>();
        private Dictionary<int, Room> _removeRooms = new Dictionary<int, Room>();

        public event VerifyRoomDelegate OnVerifyRoom;
        private bool HandleOnVerifyRoomEvent(Room roomInfo, out string message)
        {
            if (OnVerifyRoom != null)
            {
                return OnVerifyRoom(roomInfo, out message);
            }
            message = "";
            return true;
        }

        #endregion

        public RoomListServer()
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = 10069;
            strategy.Sync = true;
            _roomServer = new NetworkServer(this, strategy);
            _roomServer.UpdateInterval = 100;
            _roomServer.OnLogicUpdate += RoomServer_OnLogicUpdate;
            _roomServer.OnClientDisconnected += RoomServer_OnClientDisconnected;

            TcpNetworkStrategy strategy2 = new TcpNetworkStrategy();
            strategy2.Port = 10070;
            strategy2.Sync = true;
            _userServer = new NetworkServer(this, strategy2);
        }

        public bool Initialize()
        {
            if (!_roomServer.Initialize()) return false;
            if (!_userServer.Initialize()) return false;
            Logger.LogInfo("Server start to work.");
            return true;
        }

        public void Run()
        {
            if (_roomServer != null) _roomServer.RunThread();
            if (_userServer!=null) _userServer.RunThread();
        }

        public void Stop()
        {
            if (_roomServer != null)
            {
                _roomServer.Stop();
                _roomServer = null;
            }
            if (_userServer != null)
            {
                _userServer.Stop();
                _userServer = null;
            }
        }

        #region RoomServerEvent
        private void RoomServer_OnLogicUpdate()
        {
            Dictionary<int, Room> removeList = new Dictionary<int, Room>();
            _removeRooms = Interlocked.Exchange(ref removeList, _removeRooms);
            foreach (int key in removeList.Keys)
            {
                _userServer.BroadCast(RoomListServerHelper.RemoveRoom(_rooms[key].Identity));
                _rooms.Remove(key);
            }
        }

        private void RoomServer_OnClientDisconnected(ClientSession session)
        {
            Room room;
            bool contains = _rooms.TryGetValue(session.SessionID, out room);
            if (contains)
            {
                _removeRooms[session.SessionID] = room;
            }
        }

        #endregion

        #region UserServerEvent

        #endregion

        #region IProtocolInterpreter 成员

        public bool InterpretMessage(int sessionID, ByteArray byteArray)
        {
            if (_roomServer != null && _userServer!=null)
            {
                return RoomListServerHelper.InterpretMessage(sessionID, byteArray, this);
            }
            return true;
        }

        #endregion

        #region IRoomListServerService 成员

        public void OnRoomLogon(int sessionID, Room roomInfo)
        {
            string message;
            if (HandleOnVerifyRoomEvent(roomInfo, out message))
            {
                roomInfo.Identity = sessionID;
                roomInfo.Address = _roomServer.GetClient(sessionID).ClientAddress.ToString();
                _rooms[sessionID] = roomInfo;

                _roomServer.Send(sessionID, RoomListServerHelper.LogonSuccess());

                _userServer.BroadCast(RoomListServerHelper.AddNewRoom(roomInfo));

                Logger.LogInfo("New room logoned, ID : {0} , Name : {1} , Address : {2}", sessionID, roomInfo.Name, roomInfo.Address);
            }
            else
            {
                _roomServer.Send(sessionID, RoomListServerHelper.LogonFail(message));

                _roomServer.Disconnect(sessionID);

                Logger.LogInfo("Room logon failed, ID : {0} , Name : {1} , Address : {2}", sessionID, roomInfo.Name, message);

            }
        }

        public void OnRoomLogout(int sessionID)
        {
            _roomServer.Disconnect(sessionID);

            Logger.LogInfo("Room Logouted, ID : {0}", sessionID);
        }

        public void OnUpdateRoomInfo(int sessionID, Room roomInfo)
        {
            roomInfo.Identity = sessionID;
            roomInfo.Address = _rooms[sessionID].Address;
            _rooms[sessionID] = roomInfo;
            _userServer.BroadCast(RoomListServerHelper.UpdateRoom(roomInfo));

            Logger.LogInfo("Room updated, ID : {0} , Name : {1}", sessionID, roomInfo.Name);
        }


        public void OnUserLogon(int sessionID)
        {
            RoomSequence rooms = new RoomSequence();
            rooms.Elements.AddRange(_rooms.Values);
            _userServer.Send(sessionID, RoomListServerHelper.AddRoomList(rooms));

            Logger.LogInfo("User Logoned, ID : {0} ", sessionID);
        }

        public void OnUserLogout(int sessionID)
        {
            _userServer.Disconnect(sessionID);
            Logger.LogInfo("User Logouted, ID : {0}", sessionID);
        }

        #endregion
    }
}
