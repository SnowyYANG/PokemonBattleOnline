
using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using NetworkLib.Utilities;
using NetworkLib;

namespace PokemonBattle.RoomList.Server
{
    

    #region Enumerations
        
    #endregion

    #region Structures
        
        public class Room
        {
            public int Identity;
            public string Name;
            public string Description;
            public byte MaxUser;
            public byte UserCount;
            public string Address;
            
            public void WriteToByteArray(ByteArray byteArray)
            {
                byteArray.WriteInt(Identity);
                byteArray.WriteUTF(Name);
                byteArray.WriteUTF(Description);
                byteArray.WriteByte(MaxUser);
                byteArray.WriteByte(UserCount);
                byteArray.WriteUTF(Address);
                
            }

            public void ReadFromByteArray(ByteArray byteArray)
            {
                Identity = byteArray.ReadInt();
                Name = byteArray.ReadUTF();
                Description = byteArray.ReadUTF();
                MaxUser = byteArray.ReadByte();
                UserCount = byteArray.ReadByte();
                Address = byteArray.ReadUTF();
                
            }
        }
    
    #endregion

    #region Sequences
        
        public class RoomSequence
        {
            
            private List<Room> _elements = new List<Room>() ;
            
            public List<Room> Elements
            {
                get { return _elements;}
            }

            
            public void WriteToByteArray(ByteArray byteArray)
            {
                byteArray.WriteInt(_elements.Count);

                foreach (Room elem in _elements)
                {
                    elem.WriteToByteArray(byteArray);
                }
            }

            public void ReadFromByteArray(ByteArray byteArray)
            {
                int cnt = byteArray.ReadInt();

                for(int i = 0; i < cnt ; i ++)
                {
                    Room elem = new Room();
		elem.ReadFromByteArray(byteArray);
                    _elements.Add(elem);
                }
            }
        }
    
    #endregion

    #region Service And Interfaces
        
        internal class EnumRoomListMethods
        {
        
            public const int Server_RoomLogon_Hash = -1267164041;
        
            public const int Server_RoomLogout_Hash = -1266377634;
        
            public const int Server_UpdateRoomInfo_Hash = -1460675192;
        
            public const int Server_UserLogon_Hash = -394619561;
        
            public const int Server_UserLogout_Hash = -389114558;
        
            public const int Client_LogonSuccess_Hash = 2061128430;
        
            public const int Client_LogonFail_Hash = -632492989;
        
            public const int Client_UpdateRoom_Hash = 382024732;
        
            public const int Client_AddNewRoom_Hash = -6811936;
        
            public const int Client_RemoveRoom_Hash = -995916257;
        
            public const int Client_AddRoomList_Hash = 1096688279;
        
        }
        
        
        public class RoomListServerHelper
        {
            
            public static ByteArray LogonSuccess()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Client_LogonSuccess_Hash );
                
                return byteArray;
            }
            
            public static ByteArray LogonFail(string message)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Client_LogonFail_Hash );
                byteArray.WriteUTF(message);
                    
                
                return byteArray;
            }
            
            public static ByteArray UpdateRoom(Room roomInfo)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Client_UpdateRoom_Hash );
                roomInfo.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray AddNewRoom(Room roomInfo)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Client_AddNewRoom_Hash );
                roomInfo.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray RemoveRoom(int identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Client_RemoveRoom_Hash );
                byteArray.WriteInt(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray AddRoomList(RoomSequence rooms)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Client_AddRoomList_Hash );
                rooms.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            

            public static bool InterpretMessage(int sessionID ,ByteArray byteArray,IRoomListServerService serverService)
            {
                byteArray.BypassHeader();
                int methodID = byteArray.ReadInt();

                switch(methodID)
                {
                
                    case EnumRoomListMethods.Server_RoomLogon_Hash:
                        return OnRoomLogon(sessionID, byteArray, serverService);
                
                    case EnumRoomListMethods.Server_RoomLogout_Hash:
                        return OnRoomLogout(sessionID, byteArray, serverService);
                
                    case EnumRoomListMethods.Server_UpdateRoomInfo_Hash:
                        return OnUpdateRoomInfo(sessionID, byteArray, serverService);
                
                    case EnumRoomListMethods.Server_UserLogon_Hash:
                        return OnUserLogon(sessionID, byteArray, serverService);
                
                    case EnumRoomListMethods.Server_UserLogout_Hash:
                        return OnUserLogout(sessionID, byteArray, serverService);
                
                }

                byteArray.Rewind();
                return false;
            }
            
            private static bool OnRoomLogon(int sessionID, ByteArray byteArray ,IRoomListServerService serverService)
                {
                Room roomInfo = new Room();
		roomInfo.ReadFromByteArray(byteArray);
                
                serverService.OnRoomLogon(sessionID, roomInfo);
                return true;
            }
            
            private static bool OnRoomLogout(int sessionID, ByteArray byteArray ,IRoomListServerService serverService)
                {
                
                serverService.OnRoomLogout(sessionID);
                return true;
            }
            
            private static bool OnUpdateRoomInfo(int sessionID, ByteArray byteArray ,IRoomListServerService serverService)
                {
                Room roomInfo = new Room();
		roomInfo.ReadFromByteArray(byteArray);
                
                serverService.OnUpdateRoomInfo(sessionID, roomInfo);
                return true;
            }
            
            private static bool OnUserLogon(int sessionID, ByteArray byteArray ,IRoomListServerService serverService)
                {
                
                serverService.OnUserLogon(sessionID);
                return true;
            }
            
            private static bool OnUserLogout(int sessionID, ByteArray byteArray ,IRoomListServerService serverService)
                {
                
                serverService.OnUserLogout(sessionID);
                return true;
            }
            
        }

        public interface IRoomListServerService
        {
            
                void OnRoomLogon(int sessionID, Room roomInfo);
            
                void OnRoomLogout(int sessionID);
            
                void OnUpdateRoomInfo(int sessionID, Room roomInfo);
            
                void OnUserLogon(int sessionID);
            
                void OnUserLogout(int sessionID);
            
        }
        
    
    #endregion
}
    