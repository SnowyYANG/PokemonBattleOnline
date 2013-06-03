
using System;
using System.Text;    
using System.Xml;
using System.IO;
using System.Collections.Generic;
using NetworkLib.Utilities;
using NetworkLib;

namespace PokemonBattle.FourPlayer.Client
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

        
        public class FourPlayerClientHelper
        {
            
            public static ByteArray Logon(int identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumFourPlayerMethods.Server_Logon_Hash );
                byteArray.WriteInt(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray SetPosition(byte position,string player)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumFourPlayerMethods.Server_SetPosition_Hash );
                byteArray.WriteByte(position);
                    
                byteArray.WriteUTF(player);
                    
                
                return byteArray;
            }
            
            public static ByteArray StartBattle()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumFourPlayerMethods.Server_StartBattle_Hash );
                
                return byteArray;
            }
            
            public static ByteArray Close()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumFourPlayerMethods.Server_Close_Hash );
                
                return byteArray;
            }
            

            public static bool InterpretMessage(int sessionID ,ByteArray byteArray,IFourPlayerClientService clientService)
            {
                byteArray.BypassHeader();
                int methodID = byteArray.ReadInt();

                switch(methodID)
                {
                
                    case EnumFourPlayerMethods.Client_SetPosition_Hash:
                        return OnSetPosition(byteArray, clientService);
                
                    case EnumFourPlayerMethods.Client_SetPositionSuccess_Hash:
                        return OnSetPositionSuccess(byteArray, clientService);
                
                    case EnumFourPlayerMethods.Client_StartBattle_Hash:
                        return OnStartBattle(byteArray, clientService);
                
                    case EnumFourPlayerMethods.Client_Close_Hash:
                        return OnClose(byteArray, clientService);
                
                }

                byteArray.Rewind();
                return false;
            }
            
            private static bool OnSetPosition(ByteArray byteArray ,IFourPlayerClientService clientService)
            {
                byte position = byteArray.ReadByte();
                string player = byteArray.ReadUTF();
                
                clientService.OnSetPosition(position,player);
                return true;
            }
            
            private static bool OnSetPositionSuccess(ByteArray byteArray ,IFourPlayerClientService clientService)
            {
                byte position = byteArray.ReadByte();
                
                clientService.OnSetPositionSuccess(position);
                return true;
            }
            
            private static bool OnStartBattle(ByteArray byteArray ,IFourPlayerClientService clientService)
            {
                int identity = byteArray.ReadInt();
                
                clientService.OnStartBattle(identity);
                return true;
            }
            
            private static bool OnClose(ByteArray byteArray ,IFourPlayerClientService clientService)
            {
                
                clientService.OnClose();
                return true;
            }
            
        }

        public interface IFourPlayerClientService
        {
            
                void OnSetPosition(byte position,string player);
            
                void OnSetPositionSuccess(byte position);
            
                void OnStartBattle(int identity);
            
                void OnClose();
            
        }
    
    #endregion
}
    