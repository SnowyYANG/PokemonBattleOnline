
using System;
using System.Text;    
using System.Xml;
using System.IO;
using System.Collections.Generic;
using NetworkLib.Utilities;
using NetworkLib;

using PokemonBattle.BattleNetwork;

        
namespace PokemonBattle.BattleRoom.Client
{
    
        
    #region Enumerations
        
        public enum UserState
        {
            Free = 419938667,
            Challenging = 222308226,
            Battling = -1069528294,
            Away = -848236796,
            WrongInput = -1
        }
        public static class UserStateHelper
        {
            public static UserState ReadFromByteArray(ByteArray byteArray)
            {
                int val = byteArray.ReadInt();
                return Parse(val);
            }
            public static UserState Parse(int val)
            {
                
                    if (val == 419938667)
                    {
                        return UserState.Free;
                    }
                
                    if (val == 222308226)
                    {
                        return UserState.Challenging;
                    }
                
                    if (val == -1069528294)
                    {
                        return UserState.Battling;
                    }
                
                    if (val == -848236796)
                    {
                        return UserState.Away;
                    }
                
                return UserState.WrongInput;
            }
            
            public static void WriteToByteArray(ByteArray byteArray, UserState  value)
            {
                byteArray.WriteInt((int)value);
            }
        }
    
        public enum BattleLinkMode
        {
            Direct = -1063922077,
            Agent = -1597211022,
            WrongInput = -1
        }
        public static class BattleLinkModeHelper
        {
            public static BattleLinkMode ReadFromByteArray(ByteArray byteArray)
            {
                int val = byteArray.ReadInt();
                return Parse(val);
            }
            public static BattleLinkMode Parse(int val)
            {
                
                    if (val == -1063922077)
                    {
                        return BattleLinkMode.Direct;
                    }
                
                    if (val == -1597211022)
                    {
                        return BattleLinkMode.Agent;
                    }
                
                return BattleLinkMode.WrongInput;
            }
            
            public static void WriteToByteArray(ByteArray byteArray, BattleLinkMode  value)
            {
                byteArray.WriteInt((int)value);
            }
        }
    
    #endregion
    
    #region Structures
        
        public class User
        {
            public int Identity;
            public string Name;
            public UserState State;
            public string Address;
            public byte ImageKey;
            public string CustomDataInfo;
            public string CustomDataHash;
            
            public void WriteToByteArray(ByteArray byteArray)
            {
                byteArray.WriteInt(Identity);
                byteArray.WriteUTF(Name);
                UserStateHelper.WriteToByteArray(byteArray,State);
                byteArray.WriteUTF(Address);
                byteArray.WriteByte(ImageKey);
                byteArray.WriteUTF(CustomDataInfo);
                byteArray.WriteUTF(CustomDataHash);
            }

            public void ReadFromByteArray(ByteArray byteArray)
            {
                Identity = byteArray.ReadInt();
                Name = byteArray.ReadUTF();
                State= UserStateHelper.ReadFromByteArray(byteArray);
                Address = byteArray.ReadUTF();
                ImageKey = byteArray.ReadByte();
                CustomDataInfo = byteArray.ReadUTF();
                CustomDataHash = byteArray.ReadUTF();
                
            }
        }
    
        public class ChallengeInfo
        {
            public BattleMode BattleMode;
            public BattleLinkMode LinkMode;
            public BattleRuleSequence Rules;
            
            public void WriteToByteArray(ByteArray byteArray)
            {
                BattleModeHelper.WriteToByteArray(byteArray,BattleMode);
                BattleLinkModeHelper.WriteToByteArray(byteArray,LinkMode);
                Rules.WriteToByteArray(byteArray);
                
            }

            public void ReadFromByteArray(ByteArray byteArray)
            {
                BattleMode= BattleModeHelper.ReadFromByteArray(byteArray);
                LinkMode= BattleLinkModeHelper.ReadFromByteArray(byteArray);
                Rules= new BattleRuleSequence();
		Rules.ReadFromByteArray(byteArray);
                
            }
        }
    
        public class ObserveInfo
        {
            public string BattleAddress;
            public int BattleIdentity;
            public byte Position;
            
            public void WriteToByteArray(ByteArray byteArray)
            {
                byteArray.WriteUTF(BattleAddress);
                byteArray.WriteInt(BattleIdentity);
                byteArray.WriteByte(Position);
                
            }

            public void ReadFromByteArray(ByteArray byteArray)
            {
                BattleAddress = byteArray.ReadUTF();
                BattleIdentity = byteArray.ReadInt();
                Position = byteArray.ReadByte();
                
            }
        }
    
        public class FourPlayerRoom
        {
            public string Name;
            public int Identity;
            public byte PlayerCount;
            
            public void WriteToByteArray(ByteArray byteArray)
            {
                byteArray.WriteUTF(Name);
                byteArray.WriteInt(Identity);
                byteArray.WriteByte(PlayerCount);
                
            }

            public void ReadFromByteArray(ByteArray byteArray)
            {
                Name = byteArray.ReadUTF();
                Identity = byteArray.ReadInt();
                PlayerCount = byteArray.ReadByte();
                
            }
        }

        public class RoomBattleSetting
        {
            public int MoveInterval;
            public bool SingleBan;
            public bool DoubleBan;
            public bool FourPlayerBan;
            public bool RandomOnly;
            public string Version;

            public void WriteToByteArray(ByteArray byteArray)
            {
                byteArray.WriteInt(MoveInterval);
                byteArray.WriteBoolean(SingleBan);
                byteArray.WriteBoolean(DoubleBan);
                byteArray.WriteBoolean(FourPlayerBan);
                byteArray.WriteBoolean(RandomOnly);
                byteArray.WriteUTF(Version);
            }

            public void ReadFromByteArray(ByteArray byteArray)
            {
                MoveInterval = byteArray.ReadInt();
                SingleBan = byteArray.ReadBoolean();
                DoubleBan = byteArray.ReadBoolean();
                FourPlayerBan = byteArray.ReadBoolean();
                RandomOnly = byteArray.ReadBoolean();
                Version = byteArray.ReadUTF();
            }
        }
    
    #endregion
    
    #region Sequences
        
        public class UserSequence
        {
            
            private List<User> _elements = new List<User>() ;
            
            public List<User> Elements
            {
                get { return _elements;}
            }

            
            public void WriteToByteArray(ByteArray byteArray)
            {
                byteArray.WriteInt(_elements.Count);

                foreach (User elem in _elements)
                {
                    elem.WriteToByteArray(byteArray);
                }
            }

            public void ReadFromByteArray(ByteArray byteArray)
            {
                int cnt = byteArray.ReadInt();

                for(int i = 0; i < cnt ; i ++)
                {
                    User elem = new User();
		elem.ReadFromByteArray(byteArray);
                    _elements.Add(elem);
                }
            }
        }
    
        public class FourPlayerRoomSequence
        {
            
            private List<FourPlayerRoom> _elements = new List<FourPlayerRoom>() ;
            
            public List<FourPlayerRoom> Elements
            {
                get { return _elements;}
            }

            
            public void WriteToByteArray(ByteArray byteArray)
            {
                byteArray.WriteInt(_elements.Count);

                foreach (FourPlayerRoom elem in _elements)
                {
                    elem.WriteToByteArray(byteArray);
                }
            }

            public void ReadFromByteArray(ByteArray byteArray)
            {
                int cnt = byteArray.ReadInt();

                for(int i = 0; i < cnt ; i ++)
                {
                    FourPlayerRoom elem = new FourPlayerRoom();
		elem.ReadFromByteArray(byteArray);
                    _elements.Add(elem);
                }
            }
        }
    
    #endregion
    
    #region Service And Interfaces
        
        
        internal class EnumRoomMethods
        {
            
            public const int Server_AcceptChallenge_Hash = -1374719948;
            
            public const int Server_RefuseChallenge_Hash = -1990948387;
            
            public const int Server_Challenge_Hash = -255202176;
            
            public const int Server_CancelChallenge_Hash = -1060517986;
            
            public const int Server_StartBattle_Hash = -357168322;
            
            public const int Server_StartFourPlayerBattle_Hash = 1051353580;
            
            public const int Server_GetObserveInfo_Hash = -935233673;
            
            public const int Server_UserLogon_Hash = -394619561;
            
            public const int Server_UserLogout_Hash = -389114558;
            
            public const int Server_UpdateUser_Hash = 1182844811;
            
            public const int Server_ReceiveBroadcastMessage_Hash = 1117450454;
            
            public const int Server_ReceiveChatMessage_Hash = 1040501440;
            
            public const int Server_ReceiveRoomCommand_Hash = 254014176;
            
            public const int Server_RegistFourPlayer_Hash = -1248496399;
            
            public const int Client_AcceptChallenge_Hash = -1374719948;
            
            public const int Client_RefuseChallenge_Hash = -1990948387;
            
            public const int Client_Challenge_Hash = -255202176;
            
            public const int Client_CancelChallenge_Hash = -1060517986;
            
            public const int Client_DirectBattle_Hash = -1293976651;
            
            public const int Client_AgentBattle_Hash = -842583856;
            
            public const int Client_ReceiveObserveInfo_Hash = 332022457;
            
            public const int Client_ReceiveBroadcastMessage_Hash = 1117450454;
            
            public const int Client_ReceiveChatMessage_Hash = 1040501440;
            
            public const int Client_LogonSuccess_Hash = 2061128430;
            
            public const int Client_LogonFail_Hash = -632492989;
            
            public const int Client_BeKicked_Hash = -1532911397;
            
            public const int Client_UpdateUser_Hash = 1182844811;
            
            public const int Client_AddNewUser_Hash = -1971873217;
            
            public const int Client_RemoveUser_Hash = -1356060584;
            
            public const int Client_AddUserList_Hash = 1926515446;
            
            public const int Client_UpdateRoomSetting_Hash = -547480323;
            
            public const int Client_RegistFourPlayerSuccess_Hash = 581905803;
            
            public const int Client_AddFourPlayerRoomList_Hash = -1166474376;
            
            public const int Client_AddFourPlayerRoom_Hash = 436036668;
            
            public const int Client_RemoveFourPlayerRoom_Hash = 580986008;
            
            public const int Client_UpdateFourPlayerRoom_Hash = 1934625535;
            
        }

        
        public class RoomClientHelper
        {
            
            public static ByteArray AcceptChallenge(int targetIdentity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_AcceptChallenge_Hash );
                byteArray.WriteInt(targetIdentity);
                    
                
                return byteArray;
            }
            
            public static ByteArray RefuseChallenge(int targetIdentity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_RefuseChallenge_Hash );
                byteArray.WriteInt(targetIdentity);
                    
                
                return byteArray;
            }
            
            public static ByteArray Challenge(int targetIdentity,ChallengeInfo info)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_Challenge_Hash );
                byteArray.WriteInt(targetIdentity);
                    
                info.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray CancelChallenge(int targetIdentity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_CancelChallenge_Hash );
                byteArray.WriteInt(targetIdentity);
                    
                
                return byteArray;
            }
            
            public static ByteArray StartBattle(int with,ChallengeInfo info)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_StartBattle_Hash );
                byteArray.WriteInt(with);
                    
                info.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray StartFourPlayerBattle(int battleIdentity,byte position)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_StartFourPlayerBattle_Hash );
                byteArray.WriteInt(battleIdentity);
                    
                byteArray.WriteByte(position);
                    
                
                return byteArray;
            }
            
            public static ByteArray GetObserveInfo(int identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_GetObserveInfo_Hash );
                byteArray.WriteInt(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray UserLogon(User info)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_UserLogon_Hash );
                info.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray UserLogout()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_UserLogout_Hash );
                
                return byteArray;
            }
            
            public static ByteArray UpdateUser(User info)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_UpdateUser_Hash );
                info.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveBroadcastMessage(string message)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_ReceiveBroadcastMessage_Hash );
                byteArray.WriteUTF(message);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveChatMessage(int to,string message)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_ReceiveChatMessage_Hash );
                byteArray.WriteInt(to);
                    
                byteArray.WriteUTF(message);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveRoomCommand(string message)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_ReceiveRoomCommand_Hash );
                byteArray.WriteUTF(message);
                    
                
                return byteArray;
            }
            
            public static ByteArray RegistFourPlayer()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Server_RegistFourPlayer_Hash );
                
                return byteArray;
            }
            

            public static bool InterpretMessage(int sessionID ,ByteArray byteArray,IRoomClientService clientService)
            {
                byteArray.BypassHeader();
                int methodID = byteArray.ReadInt();

                switch(methodID)
                {
                
                    case EnumRoomMethods.Client_AcceptChallenge_Hash:
                        return OnAcceptChallenge(byteArray, clientService);
                
                    case EnumRoomMethods.Client_RefuseChallenge_Hash:
                        return OnRefuseChallenge(byteArray, clientService);
                
                    case EnumRoomMethods.Client_Challenge_Hash:
                        return OnChallenge(byteArray, clientService);
                
                    case EnumRoomMethods.Client_CancelChallenge_Hash:
                        return OnCancelChallenge(byteArray, clientService);
                
                    case EnumRoomMethods.Client_DirectBattle_Hash:
                        return OnDirectBattle(byteArray, clientService);
                
                    case EnumRoomMethods.Client_AgentBattle_Hash:
                        return OnAgentBattle(byteArray, clientService);
                
                    case EnumRoomMethods.Client_ReceiveObserveInfo_Hash:
                        return OnReceiveObserveInfo(byteArray, clientService);
                
                    case EnumRoomMethods.Client_ReceiveBroadcastMessage_Hash:
                        return OnReceiveBroadcastMessage(byteArray, clientService);
                
                    case EnumRoomMethods.Client_ReceiveChatMessage_Hash:
                        return OnReceiveChatMessage(byteArray, clientService);
                
                    case EnumRoomMethods.Client_LogonSuccess_Hash:
                        return OnLogonSuccess(byteArray, clientService);
                
                    case EnumRoomMethods.Client_LogonFail_Hash:
                        return OnLogonFail(byteArray, clientService);
                
                    case EnumRoomMethods.Client_BeKicked_Hash:
                        return OnBeKicked(byteArray, clientService);
                
                    case EnumRoomMethods.Client_UpdateUser_Hash:
                        return OnUpdateUser(byteArray, clientService);
                
                    case EnumRoomMethods.Client_AddNewUser_Hash:
                        return OnAddNewUser(byteArray, clientService);
                
                    case EnumRoomMethods.Client_RemoveUser_Hash:
                        return OnRemoveUser(byteArray, clientService);
                
                    case EnumRoomMethods.Client_AddUserList_Hash:
                        return OnAddUserList(byteArray, clientService);
                
                    case EnumRoomMethods.Client_UpdateRoomSetting_Hash:
                        return OnUpdateRoomSetting(byteArray, clientService);
                
                    case EnumRoomMethods.Client_RegistFourPlayerSuccess_Hash:
                        return OnRegistFourPlayerSuccess(byteArray, clientService);
                
                    case EnumRoomMethods.Client_AddFourPlayerRoomList_Hash:
                        return OnAddFourPlayerRoomList(byteArray, clientService);
                
                    case EnumRoomMethods.Client_AddFourPlayerRoom_Hash:
                        return OnAddFourPlayerRoom(byteArray, clientService);
                
                    case EnumRoomMethods.Client_RemoveFourPlayerRoom_Hash:
                        return OnRemoveFourPlayerRoom(byteArray, clientService);
                
                    case EnumRoomMethods.Client_UpdateFourPlayerRoom_Hash:
                        return OnUpdateFourPlayerRoom(byteArray, clientService);
                
                }

                byteArray.Rewind();
                return false;
            }
            
            private static bool OnAcceptChallenge(ByteArray byteArray ,IRoomClientService clientService)
            {
                int from = byteArray.ReadInt();
                
                clientService.OnAcceptChallenge(from);
                return true;
            }
            
            private static bool OnRefuseChallenge(ByteArray byteArray ,IRoomClientService clientService)
            {
                int from = byteArray.ReadInt();
                
                clientService.OnRefuseChallenge(from);
                return true;
            }
            
            private static bool OnChallenge(ByteArray byteArray ,IRoomClientService clientService)
            {
                int from = byteArray.ReadInt();
                ChallengeInfo info = new ChallengeInfo();
		info.ReadFromByteArray(byteArray);
                
                clientService.OnChallenge(from,info);
                return true;
            }
            
            private static bool OnCancelChallenge(ByteArray byteArray ,IRoomClientService clientService)
            {
                int from = byteArray.ReadInt();
                
                clientService.OnCancelChallenge(from);
                return true;
            }
            
            private static bool OnDirectBattle(ByteArray byteArray ,IRoomClientService clientService)
            {
                int server = byteArray.ReadInt();
                BattleMode battleMode = BattleModeHelper.ReadFromByteArray(byteArray);
                
                clientService.OnDirectBattle(server,battleMode);
                return true;
            }
            
            private static bool OnAgentBattle(ByteArray byteArray ,IRoomClientService clientService)
            {
                int identity = byteArray.ReadInt();
                byte playerPosition = byteArray.ReadByte();
                BattleMode battleMode = BattleModeHelper.ReadFromByteArray(byteArray);
                
                clientService.OnAgentBattle(identity,playerPosition,battleMode);
                return true;
            }
            
            private static bool OnReceiveObserveInfo(ByteArray byteArray ,IRoomClientService clientService)
            {
                ObserveInfo info = new ObserveInfo();
		info.ReadFromByteArray(byteArray);
                
                clientService.OnReceiveObserveInfo(info);
                return true;
            }
            
            private static bool OnReceiveBroadcastMessage(ByteArray byteArray ,IRoomClientService clientService)
            {
                string message = byteArray.ReadUTF();
                
                clientService.OnReceiveBroadcastMessage(message);
                return true;
            }
            
            private static bool OnReceiveChatMessage(ByteArray byteArray ,IRoomClientService clientService)
            {
                int from = byteArray.ReadInt();
                string message = byteArray.ReadUTF();
                
                clientService.OnReceiveChatMessage(from,message);
                return true;
            }
            
            private static bool OnLogonSuccess(ByteArray byteArray ,IRoomClientService clientService)
            {
                User info = new User();
		info.ReadFromByteArray(byteArray);
                
                clientService.OnLogonSuccess(info);
                return true;
            }
            
            private static bool OnLogonFail(ByteArray byteArray ,IRoomClientService clientService)
            {
                string message = byteArray.ReadUTF();
                
                clientService.OnLogonFail(message);
                return true;
            }
            
            private static bool OnBeKicked(ByteArray byteArray ,IRoomClientService clientService)
            {
                
                clientService.OnBeKicked();
                return true;
            }
            
            private static bool OnUpdateUser(ByteArray byteArray ,IRoomClientService clientService)
            {
                User userInfo = new User();
		userInfo.ReadFromByteArray(byteArray);
                
                clientService.OnUpdateUser(userInfo);
                return true;
            }
            
            private static bool OnAddNewUser(ByteArray byteArray ,IRoomClientService clientService)
            {
                User userInfo = new User();
		userInfo.ReadFromByteArray(byteArray);
                
                clientService.OnAddNewUser(userInfo);
                return true;
            }
            
            private static bool OnRemoveUser(ByteArray byteArray ,IRoomClientService clientService)
            {
                int identity = byteArray.ReadInt();
                
                clientService.OnRemoveUser(identity);
                return true;
            }
            
            private static bool OnAddUserList(ByteArray byteArray ,IRoomClientService clientService)
            {
                UserSequence users = new UserSequence();
		users.ReadFromByteArray(byteArray);
                
                clientService.OnAddUserList(users);
                return true;
            }
            
            private static bool OnUpdateRoomSetting(ByteArray byteArray ,IRoomClientService clientService)
            {
                RoomBattleSetting setting = new RoomBattleSetting();
		setting.ReadFromByteArray(byteArray);
                
                clientService.OnUpdateRoomSetting(setting);
                return true;
            }
            
            private static bool OnRegistFourPlayerSuccess(ByteArray byteArray ,IRoomClientService clientService)
            {
                int identity = byteArray.ReadInt();
                
                clientService.OnRegistFourPlayerSuccess(identity);
                return true;
            }
            
            private static bool OnAddFourPlayerRoomList(ByteArray byteArray ,IRoomClientService clientService)
            {
                FourPlayerRoomSequence rooms = new FourPlayerRoomSequence();
		rooms.ReadFromByteArray(byteArray);
                
                clientService.OnAddFourPlayerRoomList(rooms);
                return true;
            }
            
            private static bool OnAddFourPlayerRoom(ByteArray byteArray ,IRoomClientService clientService)
            {
                int identity = byteArray.ReadInt();
                string host = byteArray.ReadUTF();
                
                clientService.OnAddFourPlayerRoom(identity,host);
                return true;
            }
            
            private static bool OnRemoveFourPlayerRoom(ByteArray byteArray ,IRoomClientService clientService)
            {
                int identity = byteArray.ReadInt();
                
                clientService.OnRemoveFourPlayerRoom(identity);
                return true;
            }
            
            private static bool OnUpdateFourPlayerRoom(ByteArray byteArray ,IRoomClientService clientService)
            {
                int identity = byteArray.ReadInt();
                byte userCount = byteArray.ReadByte();
                
                clientService.OnUpdateFourPlayerRoom(identity,userCount);
                return true;
            }
            
        }

        public interface IRoomClientService
        {
            
                void OnAcceptChallenge(int from);
            
                void OnRefuseChallenge(int from);
            
                void OnChallenge(int from,ChallengeInfo info);
            
                void OnCancelChallenge(int from);
            
                void OnDirectBattle(int server,BattleMode battleMode);
            
                void OnAgentBattle(int identity,byte playerPosition,BattleMode battleMode);
            
                void OnReceiveObserveInfo(ObserveInfo info);
            
                void OnReceiveBroadcastMessage(string message);
            
                void OnReceiveChatMessage(int from,string message);
            
                void OnLogonSuccess(User info);
            
                void OnLogonFail(string message);
            
                void OnBeKicked();
            
                void OnUpdateUser(User userInfo);
            
                void OnAddNewUser(User userInfo);
            
                void OnRemoveUser(int identity);
            
                void OnAddUserList(UserSequence users);
            
                void OnUpdateRoomSetting(RoomBattleSetting setting);
            
                void OnRegistFourPlayerSuccess(int identity);
            
                void OnAddFourPlayerRoomList(FourPlayerRoomSequence rooms);
            
                void OnAddFourPlayerRoom(int identity,string host);
            
                void OnRemoveFourPlayerRoom(int identity);
            
                void OnUpdateFourPlayerRoom(int identity,byte userCount);
            
        }
    
    #endregion
}
    