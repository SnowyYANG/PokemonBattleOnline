
using System;
using System.Text;    
using System.Xml;
using System.IO;
using System.Collections.Generic;
using NetworkLib.Utilities;
using NetworkLib;

namespace PokemonBattle.BattleNetwork.Client
{
   
    
    #region Service And Interfaces
        
        
        internal class EnumPokemonBattleMethods
        {
            
            public const int Server_Logon_Hash = 1539205103;
            
            public const int Server_ReceiveMove_Hash = 1712670317;
            
            public const int Server_ReceiveTeam_Hash = -1287328945;
            
            public const int Server_Exit_Hash = -1158854104;
            
            public const int Server_TimeUp_Hash = -1110713650;
            
            public const int Server_ReceiveTieMessage_Hash = 136563185;
            
            public const int Server_RegistObsever_Hash = -774376481;
            
            public const int Server_ReceiveBattleInfo_Hash = 1369483516;
            
            public const int Server_ReceiveBattleSnapshot_Hash = 1241333263;
            
            public const int Client_LogonSuccess_Hash = 2061128430;
            
            public const int Client_LogonFail_Hash = -632492989;
            
            public const int Client_ReceiveRandomSeed_Hash = 410557355;
            
            public const int Client_ReceiveMove_Hash = 1712670317;
            
            public const int Client_ReceiveTeam_Hash = -1287328945;
            
            public const int Client_ReceiveRules_Hash = -274250105;
            
            public const int Client_ReceiveTieMessage_Hash = 136563185;
            
            public const int Client_Exit_Hash = -1158854104;
            
            public const int Client_TimeUp_Hash = -1110713650;
            
            public const int Client_RegistObsever_Hash = -774376481;
            
            public const int Client_ReceiveBattleInfo_Hash = 1369483516;
            
            public const int Client_ReceiveBattleSnapshot_Hash = 1241333263;
            
        }

        
        public class PokemonBattleClientHelper
        {
            
            public static ByteArray Logon(string identity,BattleMode modeInfo,string versionInfo)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Server_Logon_Hash );
                byteArray.WriteUTF(identity);
                    
                BattleModeHelper.WriteToByteArray(byteArray,modeInfo);
                    
                byteArray.WriteUTF(versionInfo);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveMove(PlayerMove move)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Server_ReceiveMove_Hash );
                move.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveTeam(byte position,string identity,ByteSequence team)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Server_ReceiveTeam_Hash );
                byteArray.WriteByte(position);
                    
                byteArray.WriteUTF(identity);
                    
                team.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray Exit(string identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Server_Exit_Hash );
                byteArray.WriteUTF(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray TimeUp(string identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Server_TimeUp_Hash );
                byteArray.WriteUTF(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveTieMessage(string identity,TieMessage message)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Server_ReceiveTieMessage_Hash );
                byteArray.WriteUTF(identity);
                    
                TieMessageHelper.WriteToByteArray(byteArray,message);
                    
                
                return byteArray;
            }
            
            public static ByteArray RegistObsever(int identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Server_RegistObsever_Hash );
                byteArray.WriteInt(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveBattleInfo(BattleInfo info)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Server_ReceiveBattleInfo_Hash );
                info.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveBattleSnapshot(BattleSnapshot snapshot)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Server_ReceiveBattleSnapshot_Hash );
                snapshot.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            

            public static bool InterpretMessage(int sessionID ,ByteArray byteArray,IPokemonBattleClientService clientService)
            {
                byteArray.BypassHeader();
                int methodID = byteArray.ReadInt();

                switch(methodID)
                {
                
                    case EnumPokemonBattleMethods.Client_LogonSuccess_Hash:
                        return OnLogonSuccess(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_LogonFail_Hash:
                        return OnLogonFail(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_ReceiveRandomSeed_Hash:
                        return OnReceiveRandomSeed(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_ReceiveMove_Hash:
                        return OnReceiveMove(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_ReceiveTeam_Hash:
                        return OnReceiveTeam(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_ReceiveRules_Hash:
                        return OnReceiveRules(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_ReceiveTieMessage_Hash:
                        return OnReceiveTieMessage(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_Exit_Hash:
                        return OnExit(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_TimeUp_Hash:
                        return OnTimeUp(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_RegistObsever_Hash:
                        return OnRegistObsever(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_ReceiveBattleInfo_Hash:
                        return OnReceiveBattleInfo(byteArray, clientService);
                
                    case EnumPokemonBattleMethods.Client_ReceiveBattleSnapshot_Hash:
                        return OnReceiveBattleSnapshot(byteArray, clientService);
                
                }

                byteArray.Rewind();
                return false;
            }
            
            private static bool OnLogonSuccess(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                
                clientService.OnLogonSuccess();
                return true;
            }
            
            private static bool OnLogonFail(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                string message = byteArray.ReadUTF();
                
                clientService.OnLogonFail(message);
                return true;
            }
            
            private static bool OnReceiveRandomSeed(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                int seed = byteArray.ReadInt();
                
                clientService.OnReceiveRandomSeed(seed);
                return true;
            }
            
            private static bool OnReceiveMove(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                PlayerMove move = new PlayerMove();
		move.ReadFromByteArray(byteArray);
                
                clientService.OnReceiveMove(move);
                return true;
            }
            
            private static bool OnReceiveTeam(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                byte position = byteArray.ReadByte();
                string identity = byteArray.ReadUTF();
                ByteSequence team = new ByteSequence();
		team.ReadFromByteArray(byteArray);
                
                clientService.OnReceiveTeam(position,identity,team);
                return true;
            }
            
            private static bool OnReceiveRules(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                BattleRuleSequence rules = new BattleRuleSequence();
		rules.ReadFromByteArray(byteArray);
                
                clientService.OnReceiveRules(rules);
                return true;
            }
            
            private static bool OnReceiveTieMessage(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                string identity = byteArray.ReadUTF();
                TieMessage message = TieMessageHelper.ReadFromByteArray(byteArray);
                
                clientService.OnReceiveTieMessage(identity,message);
                return true;
            }
            
            private static bool OnExit(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                string identity = byteArray.ReadUTF();
                
                clientService.OnExit(identity);
                return true;
            }
            
            private static bool OnTimeUp(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                string identity = byteArray.ReadUTF();
                
                clientService.OnTimeUp(identity);
                return true;
            }
            
            private static bool OnRegistObsever(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                int identity = byteArray.ReadInt();
                
                clientService.OnRegistObsever(identity);
                return true;
            }
            
            private static bool OnReceiveBattleInfo(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                BattleInfo info = new BattleInfo();
		info.ReadFromByteArray(byteArray);
                
                clientService.OnReceiveBattleInfo(info);
                return true;
            }
            
            private static bool OnReceiveBattleSnapshot(ByteArray byteArray ,IPokemonBattleClientService clientService)
            {
                BattleSnapshot snapshot = new BattleSnapshot();
		snapshot.ReadFromByteArray(byteArray);
                
                clientService.OnReceiveBattleSnapshot(snapshot);
                return true;
            }
            
        }

        public interface IPokemonBattleClientService
        {
            
                void OnLogonSuccess();
            
                void OnLogonFail(string message);
            
                void OnReceiveRandomSeed(int seed);
            
                void OnReceiveMove(PlayerMove move);
            
                void OnReceiveTeam(byte position,string identity,ByteSequence team);
            
                void OnReceiveRules(BattleRuleSequence rules);
            
                void OnReceiveTieMessage(string identity,TieMessage message);
            
                void OnExit(string identity);
            
                void OnTimeUp(string identity);
            
                void OnRegistObsever(int identity);
            
                void OnReceiveBattleInfo(BattleInfo info);
            
                void OnReceiveBattleSnapshot(BattleSnapshot snapshot);
            
        }
    
    #endregion
}
    