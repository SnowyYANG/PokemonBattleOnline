
using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using NetworkLib.Utilities;
using NetworkLib;

namespace PokemonBattle.FourPlayer.Server
{
    

    #region Enumerations
        
    #endregion

    #region Structures
        
    #endregion

    #region Sequences
        
    #endregion

    #region Service And Interfaces
        
        internal class EnumFourPlayerMethods
        {
        
            public const int Server_Logon_Hash = 1539205103;
        
            public const int Server_SetPosition_Hash = 2127083899;
        
            public const int Server_StartBattle_Hash = -357168322;
        
            public const int Server_Close_Hash = -811482585;
        
            public const int Client_SetPosition_Hash = 2127083899;
        
            public const int Client_SetPositionSuccess_Hash = 2112945901;
        
            public const int Client_StartBattle_Hash = -357168322;
        
            public const int Client_Close_Hash = -811482585;
        
        }
        
        
        public class FourPlayerServerHelper
        {
            
            public static ByteArray SetPosition(byte position,string player)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumFourPlayerMethods.Client_SetPosition_Hash );
                byteArray.WriteByte(position);
                    
                byteArray.WriteUTF(player);
                    
                
                return byteArray;
            }
            
            public static ByteArray SetPositionSuccess(byte position)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumFourPlayerMethods.Client_SetPositionSuccess_Hash );
                byteArray.WriteByte(position);
                    
                
                return byteArray;
            }
            
            public static ByteArray StartBattle(int identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumFourPlayerMethods.Client_StartBattle_Hash );
                byteArray.WriteInt(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray Close()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumFourPlayerMethods.Client_Close_Hash );
                
                return byteArray;
            }
            

            public static bool InterpretMessage(int sessionID ,ByteArray byteArray,IFourPlayerServerService serverService)
            {
                byteArray.BypassHeader();
                int methodID = byteArray.ReadInt();

                switch(methodID)
                {
                
                    case EnumFourPlayerMethods.Server_Logon_Hash:
                        return OnLogon(sessionID, byteArray, serverService);
                
                    case EnumFourPlayerMethods.Server_SetPosition_Hash:
                        return OnSetPosition(sessionID, byteArray, serverService);
                
                    case EnumFourPlayerMethods.Server_StartBattle_Hash:
                        return OnStartBattle(sessionID, byteArray, serverService);
                
                    case EnumFourPlayerMethods.Server_Close_Hash:
                        return OnClose(sessionID, byteArray, serverService);
                
                }

                byteArray.Rewind();
                return false;
            }
            
            private static bool OnLogon(int sessionID, ByteArray byteArray ,IFourPlayerServerService serverService)
                {
                int identity = byteArray.ReadInt();
                
                serverService.OnLogon(sessionID, identity);
                return true;
            }
            
            private static bool OnSetPosition(int sessionID, ByteArray byteArray ,IFourPlayerServerService serverService)
                {
                byte position = byteArray.ReadByte();
                string player = byteArray.ReadUTF();
                
                serverService.OnSetPosition(sessionID, position, player);
                return true;
            }
            
            private static bool OnStartBattle(int sessionID, ByteArray byteArray ,IFourPlayerServerService serverService)
                {
                
                serverService.OnStartBattle(sessionID);
                return true;
            }
            
            private static bool OnClose(int sessionID, ByteArray byteArray ,IFourPlayerServerService serverService)
                {
                
                serverService.OnClose(sessionID);
                return true;
            }
            
        }

        public interface IFourPlayerServerService
        {
            
                void OnLogon(int sessionID, int identity);
            
                void OnSetPosition(int sessionID, byte position, string player);
            
                void OnStartBattle(int sessionID);
            
                void OnClose(int sessionID);
            
        }
        
    
    #endregion
}
    