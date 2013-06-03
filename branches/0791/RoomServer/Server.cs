
using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using NetworkLib.Utilities;
using NetworkLib;

using PokemonBattle.BattleNetwork;

        
namespace PokemonBattle.BattleRoom.Server
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
        
        
        public class RoomServerHelper
        {
            
            public static ByteArray AcceptChallenge(int from)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_AcceptChallenge_Hash );
                byteArray.WriteInt(from);
                    
                
                return byteArray;
            }
            
            public static ByteArray RefuseChallenge(int from)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_RefuseChallenge_Hash );
                byteArray.WriteInt(from);
                    
                
                return byteArray;
            }
            
            public static ByteArray Challenge(int from,ChallengeInfo info)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_Challenge_Hash );
                byteArray.WriteInt(from);
                    
                info.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray CancelChallenge(int from)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_CancelChallenge_Hash );
                byteArray.WriteInt(from);
                    
                
                return byteArray;
            }
            
            public static ByteArray DirectBattle(int server,BattleMode battleMode)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_DirectBattle_Hash );
                byteArray.WriteInt(server);
                    
                BattleModeHelper.WriteToByteArray(byteArray,battleMode);
                    
                
                return byteArray;
            }
            
            public static ByteArray AgentBattle(int identity,byte playerPosition,BattleMode battleMode)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_AgentBattle_Hash );
                byteArray.WriteInt(identity);
                    
                byteArray.WriteByte(playerPosition);
                    
                BattleModeHelper.WriteToByteArray(byteArray,battleMode);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveObserveInfo(ObserveInfo info)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_ReceiveObserveInfo_Hash );
                info.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveBroadcastMessage(string message)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_ReceiveBroadcastMessage_Hash );
                byteArray.WriteUTF(message);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveChatMessage(int from,string message)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_ReceiveChatMessage_Hash );
                byteArray.WriteInt(from);
                    
                byteArray.WriteUTF(message);
                    
                
                return byteArray;
            }
            
            public static ByteArray LogonSuccess(User info)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_LogonSuccess_Hash );
                info.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray LogonFail(string message)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_LogonFail_Hash );
                byteArray.WriteUTF(message);
                    
                
                return byteArray;
            }
            
            public static ByteArray BeKicked()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_BeKicked_Hash );
                
                return byteArray;
            }
            
            public static ByteArray UpdateUser(User userInfo)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_UpdateUser_Hash );
                userInfo.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray AddNewUser(User userInfo)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_AddNewUser_Hash );
                userInfo.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray RemoveUser(int identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_RemoveUser_Hash );
                byteArray.WriteInt(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray AddUserList(UserSequence users)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_AddUserList_Hash );
                users.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray UpdateRoomSetting(RoomBattleSetting setting)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_UpdateRoomSetting_Hash );
                setting.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray RegistFourPlayerSuccess(int identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_RegistFourPlayerSuccess_Hash );
                byteArray.WriteInt(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray AddFourPlayerRoomList(FourPlayerRoomSequence rooms)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_AddFourPlayerRoomList_Hash );
                rooms.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray AddFourPlayerRoom(int identity,string host)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_AddFourPlayerRoom_Hash );
                byteArray.WriteInt(identity);
                    
                byteArray.WriteUTF(host);
                    
                
                return byteArray;
            }
            
            public static ByteArray RemoveFourPlayerRoom(int identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_RemoveFourPlayerRoom_Hash );
                byteArray.WriteInt(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray UpdateFourPlayerRoom(int identity,byte userCount)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumRoomMethods.Client_UpdateFourPlayerRoom_Hash );
                byteArray.WriteInt(identity);
                    
                byteArray.WriteByte(userCount);
                    
                
                return byteArray;
            }
            

            public static bool InterpretMessage(int sessionID ,ByteArray byteArray,IRoomServerService serverService)
            {
                byteArray.BypassHeader();
                int methodID = byteArray.ReadInt();

                switch(methodID)
                {
                
                    case EnumRoomMethods.Server_AcceptChallenge_Hash:
                        return OnAcceptChallenge(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_RefuseChallenge_Hash:
                        return OnRefuseChallenge(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_Challenge_Hash:
                        return OnChallenge(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_CancelChallenge_Hash:
                        return OnCancelChallenge(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_StartBattle_Hash:
                        return OnStartBattle(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_StartFourPlayerBattle_Hash:
                        return OnStartFourPlayerBattle(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_GetObserveInfo_Hash:
                        return OnGetObserveInfo(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_UserLogon_Hash:
                        return OnUserLogon(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_UserLogout_Hash:
                        return OnUserLogout(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_UpdateUser_Hash:
                        return OnUpdateUser(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_ReceiveBroadcastMessage_Hash:
                        return OnReceiveBroadcastMessage(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_ReceiveChatMessage_Hash:
                        return OnReceiveChatMessage(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_ReceiveRoomCommand_Hash:
                        return OnReceiveRoomCommand(sessionID, byteArray, serverService);
                
                    case EnumRoomMethods.Server_RegistFourPlayer_Hash:
                        return OnRegistFourPlayer(sessionID, byteArray, serverService);
                
                }

                byteArray.Rewind();
                return false;
            }
            
            private static bool OnAcceptChallenge(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                int targetIdentity = byteArray.ReadInt();
                
                serverService.OnAcceptChallenge(sessionID, targetIdentity);
                return true;
            }
            
            private static bool OnRefuseChallenge(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                int targetIdentity = byteArray.ReadInt();
                
                serverService.OnRefuseChallenge(sessionID, targetIdentity);
                return true;
            }
            
            private static bool OnChallenge(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                int targetIdentity = byteArray.ReadInt();
                ChallengeInfo info = new ChallengeInfo();
		info.ReadFromByteArray(byteArray);
                
                serverService.OnChallenge(sessionID, targetIdentity, info);
                return true;
            }
            
            private static bool OnCancelChallenge(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                int targetIdentity = byteArray.ReadInt();
                
                serverService.OnCancelChallenge(sessionID, targetIdentity);
                return true;
            }
            
            private static bool OnStartBattle(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                int with = byteArray.ReadInt();
                ChallengeInfo info = new ChallengeInfo();
		info.ReadFromByteArray(byteArray);
                
                serverService.OnStartBattle(sessionID, with, info);
                return true;
            }
            
            private static bool OnStartFourPlayerBattle(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                int battleIdentity = byteArray.ReadInt();
                byte position = byteArray.ReadByte();
                
                serverService.OnStartFourPlayerBattle(sessionID, battleIdentity, position);
                return true;
            }
            
            private static bool OnGetObserveInfo(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                int identity = byteArray.ReadInt();
                
                serverService.OnGetObserveInfo(sessionID, identity);
                return true;
            }
            
            private static bool OnUserLogon(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                User info = new User();
		info.ReadFromByteArray(byteArray);
                
                serverService.OnUserLogon(sessionID, info);
                return true;
            }
            
            private static bool OnUserLogout(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                
                serverService.OnUserLogout(sessionID);
                return true;
            }
            
            private static bool OnUpdateUser(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                User info = new User();
		info.ReadFromByteArray(byteArray);
                
                serverService.OnUpdateUser(sessionID, info);
                return true;
            }
            
            private static bool OnReceiveBroadcastMessage(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                string message = byteArray.ReadUTF();
                
                serverService.OnReceiveBroadcastMessage(sessionID, message);
                return true;
            }
            
            private static bool OnReceiveChatMessage(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                int to = byteArray.ReadInt();
                string message = byteArray.ReadUTF();
                
                serverService.OnReceiveChatMessage(sessionID, to, message);
                return true;
            }
            
            private static bool OnReceiveRoomCommand(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                string message = byteArray.ReadUTF();
                
                serverService.OnReceiveRoomCommand(sessionID, message);
                return true;
            }
            
            private static bool OnRegistFourPlayer(int sessionID, ByteArray byteArray ,IRoomServerService serverService)
                {
                
                serverService.OnRegistFourPlayer(sessionID);
                return true;
            }
            
        }

        public interface IRoomServerService
        {
            
                void OnAcceptChallenge(int sessionID, int targetIdentity);
            
                void OnRefuseChallenge(int sessionID, int targetIdentity);
            
                void OnChallenge(int sessionID, int targetIdentity, ChallengeInfo info);
            
                void OnCancelChallenge(int sessionID, int targetIdentity);
            
                void OnStartBattle(int sessionID, int with, ChallengeInfo info);
            
                void OnStartFourPlayerBattle(int sessionID, int battleIdentity, byte position);
            
                void OnGetObserveInfo(int sessionID, int identity);
            
                void OnUserLogon(int sessionID, User info);
            
                void OnUserLogout(int sessionID);
            
                void OnUpdateUser(int sessionID, User info);
            
                void OnReceiveBroadcastMessage(int sessionID, string message);
            
                void OnReceiveChatMessage(int sessionID, int to, string message);
            
                void OnReceiveRoomCommand(int sessionID, string message);
            
                void OnRegistFourPlayer(int sessionID);
            
        }
        
    
    #endregion
}
    