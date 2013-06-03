
using System;
using System.Text;    
using System.Xml;
using System.IO;
using System.Collections.Generic;
using NetworkLib.Utilities;
using NetworkLib;

namespace PokemonBattle.RoomList.Client
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

        
        public class RoomListClientHelper
        {
            
            public static ByteArray RoomLogon(Room roomInfo)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Server_RoomLogon_Hash );
                roomInfo.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray RoomLogout()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Server_RoomLogout_Hash );
                
                return byteArray;
            }
            
            public static ByteArray UpdateRoomInfo(Room roomInfo)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Server_UpdateRoomInfo_Hash );
                roomInfo.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray UserLogon()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Server_UserLogon_Hash );
                
                return byteArray;
            }
            
            public static ByteArray UserLogout()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomListMethods.Server_UserLogout_Hash );
                
                return byteArray;
            }
            

            public static bool InterpretMessage(int sessionID ,ByteArray byteArray,IRoomListClientService clientService)
            {
                byteArray.BypassHeader();
                int methodID = byteArray.ReadInt();

                switch(methodID)
                {
                
                    case EnumRoomListMethods.Client_LogonSuccess_Hash:
                        return OnLogonSuccess(byteArray, clientService);
                
                    case EnumRoomListMethods.Client_LogonFail_Hash:
                        return OnLogonFail(byteArray, clientService);
                
                    case EnumRoomListMethods.Client_UpdateRoom_Hash:
                        return OnUpdateRoom(byteArray, clientService);
                
                    case EnumRoomListMethods.Client_AddNewRoom_Hash:
                        return OnAddNewRoom(byteArray, clientService);
                
                    case EnumRoomListMethods.Client_RemoveRoom_Hash:
                        return OnRemoveRoom(byteArray, clientService);
                
                    case EnumRoomListMethods.Client_AddRoomList_Hash:
                        return OnAddRoomList(byteArray, clientService);
                
                }

                byteArray.Rewind();
                return false;
            }
            
            private static bool OnLogonSuccess(ByteArray byteArray ,IRoomListClientService clientService)
            {
                
                clientService.OnLogonSuccess();
                return true;
            }
            
            private static bool OnLogonFail(ByteArray byteArray ,IRoomListClientService clientService)
            {
                string message = byteArray.ReadUTF();
                
                clientService.OnLogonFail(message);
                return true;
            }
            
            private static bool OnUpdateRoom(ByteArray byteArray ,IRoomListClientService clientService)
            {
                Room roomInfo = new Room();
		roomInfo.ReadFromByteArray(byteArray);
                
                clientService.OnUpdateRoom(roomInfo);
                return true;
            }
            
            private static bool OnAddNewRoom(ByteArray byteArray ,IRoomListClientService clientService)
            {
                Room roomInfo = new Room();
		roomInfo.ReadFromByteArray(byteArray);
                
                clientService.OnAddNewRoom(roomInfo);
                return true;
            }
            
            private static bool OnRemoveRoom(ByteArray byteArray ,IRoomListClientService clientService)
            {
                int identity = byteArray.ReadInt();
                
                clientService.OnRemoveRoom(identity);
                return true;
            }
            
            private static bool OnAddRoomList(ByteArray byteArray ,IRoomListClientService clientService)
            {
                RoomSequence rooms = new RoomSequence();
		rooms.ReadFromByteArray(byteArray);
                
                clientService.OnAddRoomList(rooms);
                return true;
            }
            
        }

        public interface IRoomListClientService
        {
            
                void OnLogonSuccess();
            
                void OnLogonFail(string message);
            
                void OnUpdateRoom(Room roomInfo);
            
                void OnAddNewRoom(Room roomInfo);
            
                void OnRemoveRoom(int identity);
            
                void OnAddRoomList(RoomSequence rooms);
            
        }
    
    #endregion
}
    