
using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using NetworkLib.Utilities;
using NetworkLib;

namespace PokemonBattle.BattleNetwork
{


    #region Enumerations

    public enum BattleMove
    {
        Attack = 868794430,
        SwapPokemon = -762866655,
        DeathSwap = -532471963,
        PassSwap = -1116926315,
        WrongInput = -1
    }
    public static class BattleMoveHelper
    {
        public static BattleMove ReadFromByteArray(ByteArray byteArray)
        {
            int val = byteArray.ReadInt();
            return Parse(val);
        }
        public static BattleMove Parse(int val)
        {

            if (val == 868794430)
            {
                return BattleMove.Attack;
            }

            if (val == -762866655)
            {
                return BattleMove.SwapPokemon;
            }

            if (val == -532471963)
            {
                return BattleMove.DeathSwap;
            }

            if (val == -1116926315)
            {
                return BattleMove.PassSwap;
            }

            return BattleMove.WrongInput;
        }

        public static void WriteToByteArray(ByteArray byteArray, BattleMove value)
        {
            byteArray.WriteInt((int)value);
        }
    }

    public enum PokemonIndex
    {
        Pokemon1OfTeam1 = 2146817333,
        Pokemon2OfTeam1 = 994235701,
        Pokemon1OfTeam2 = 580733392,
        Pokemon2OfTeam2 = -571848240,
        WrongInput = -1
    }
    public static class PokemonIndexHelper
    {
        public static PokemonIndex ReadFromByteArray(ByteArray byteArray)
        {
            int val = byteArray.ReadInt();
            return Parse(val);
        }
        public static PokemonIndex Parse(int val)
        {

            if (val == 2146817333)
            {
                return PokemonIndex.Pokemon1OfTeam1;
            }

            if (val == 994235701)
            {
                return PokemonIndex.Pokemon2OfTeam1;
            }

            if (val == 580733392)
            {
                return PokemonIndex.Pokemon1OfTeam2;
            }

            if (val == -571848240)
            {
                return PokemonIndex.Pokemon2OfTeam2;
            }

            return PokemonIndex.WrongInput;
        }

        public static void WriteToByteArray(ByteArray byteArray, PokemonIndex value)
        {
            byteArray.WriteInt((int)value);
        }
    }

    public enum TargetIndex
    {
        DefaultTarget = -1865455426,
        Opponent1 = -536926594,
        Opponent2 = -536926597,
        TeamFriend = -1044722981,
        Self = -710105077,
        Random = -353152975,
        WrongInput = -1
    }
    public static class TargetIndexHelper
    {
        public static TargetIndex ReadFromByteArray(ByteArray byteArray)
        {
            int val = byteArray.ReadInt();
            return Parse(val);
        }
        public static TargetIndex Parse(int val)
        {

            if (val == -1865455426)
            {
                return TargetIndex.DefaultTarget;
            }

            if (val == -536926594)
            {
                return TargetIndex.Opponent1;
            }

            if (val == -536926597)
            {
                return TargetIndex.Opponent2;
            }

            if (val == -1044722981)
            {
                return TargetIndex.TeamFriend;
            }

            if (val == -710105077)
            {
                return TargetIndex.Self;
            }

            if (val == -353152975)
            {
                return TargetIndex.Random;
            }

            return TargetIndex.WrongInput;
        }

        public static void WriteToByteArray(ByteArray byteArray, TargetIndex value)
        {
            byteArray.WriteInt((int)value);
        }
    }

    public enum BattleMode
    {
        Single = 1234456403,
        Double = 1235498031,
        Double_4P = 1451510949,
        WrongInput = -1
    }
    public static class BattleModeHelper
    {
        public static BattleMode ReadFromByteArray(ByteArray byteArray)
        {
            int val = byteArray.ReadInt();
            return Parse(val);
        }
        public static BattleMode Parse(int val)
        {

            if (val == 1234456403)
            {
                return BattleMode.Single;
            }

            if (val == 1235498031)
            {
                return BattleMode.Double;
            }

            if (val == 1451510949)
            {
                return BattleMode.Double_4P;
            }

            return BattleMode.WrongInput;
        }

        public static void WriteToByteArray(ByteArray byteArray, BattleMode value)
        {
            byteArray.WriteInt((int)value);
        }
    }

    public enum TieMessage
    {
        TieRequest = -1115504277,
        AgreeTie = -803423805,
        RefuseTie = -1091278321,
        Fail = 1605126695,
        WrongInput = -1
    }
    public static class TieMessageHelper
    {
        public static TieMessage ReadFromByteArray(ByteArray byteArray)
        {
            int val = byteArray.ReadInt();
            return Parse(val);
        }
        public static TieMessage Parse(int val)
        {

            if (val == -1115504277)
            {
                return TieMessage.TieRequest;
            }

            if (val == -803423805)
            {
                return TieMessage.AgreeTie;
            }

            if (val == -1091278321)
            {
                return TieMessage.RefuseTie;
            }

            if (val == 1605126695)
            {
                return TieMessage.Fail;
            }

            return TieMessage.WrongInput;
        }

        public static void WriteToByteArray(ByteArray byteArray, TieMessage value)
        {
            byteArray.WriteInt((int)value);
        }
    }

    public enum BattleRule
    {
        PPUp = 151839857,
        Random = -353152975,
        WrongInput = -1
    }
    public static class BattleRuleHelper
    {
        public static BattleRule ReadFromByteArray(ByteArray byteArray)
        {
            int val = byteArray.ReadInt();
            return Parse(val);
        }
        public static BattleRule Parse(int val)
        {

            if (val == 151839857)
            {
                return BattleRule.PPUp;
            }

            if (val == -353152975)
            {
                return BattleRule.Random;
            }

            return BattleRule.WrongInput;
        }

        public static void WriteToByteArray(ByteArray byteArray, BattleRule value)
        {
            byteArray.WriteInt((int)value);
        }
    }

    public enum BattleTerrain
    {
        Stadium = -1444406344,
        Grass = 372943555,
        Flat = -803934465,
        Sand = 835293505,
        Mountain = 984446105,
        Cave = -1859589031,
        Water = 78692563,
        SnowField = -1106010300,
        WrongInput = -1
    }
    public static class BattleTerrainHelper
    {
        public static BattleTerrain ReadFromByteArray(ByteArray byteArray)
        {
            int val = byteArray.ReadInt();
            return Parse(val);
        }
        public static BattleTerrain Parse(int val)
        {

            if (val == -1444406344)
            {
                return BattleTerrain.Stadium;
            }

            if (val == 372943555)
            {
                return BattleTerrain.Grass;
            }

            if (val == -803934465)
            {
                return BattleTerrain.Flat;
            }

            if (val == 835293505)
            {
                return BattleTerrain.Sand;
            }

            if (val == 984446105)
            {
                return BattleTerrain.Mountain;
            }

            if (val == -1859589031)
            {
                return BattleTerrain.Cave;
            }

            if (val == 78692563)
            {
                return BattleTerrain.Water;
            }

            if (val == -1106010300)
            {
                return BattleTerrain.SnowField;
            }

            return BattleTerrain.WrongInput;
        }

        public static void WriteToByteArray(ByteArray byteArray, BattleTerrain value)
        {
            byteArray.WriteInt((int)value);
        }
    }

    #endregion

    #region Structures

    public class PlayerMove
    {
        public string Player;
        public BattleMove Move;
        public PokemonIndex Pokemon;
        public TargetIndex Target;
        public byte MoveIndex;

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteUTF(Player);
            BattleMoveHelper.WriteToByteArray(byteArray, Move);
            PokemonIndexHelper.WriteToByteArray(byteArray, Pokemon);
            TargetIndexHelper.WriteToByteArray(byteArray, Target);
            byteArray.WriteByte(MoveIndex);

        }

        public void ReadFromByteArray(ByteArray byteArray)
        {
            Player = byteArray.ReadUTF();
            Move = BattleMoveHelper.ReadFromByteArray(byteArray);
            Pokemon = PokemonIndexHelper.ReadFromByteArray(byteArray);
            Target = TargetIndexHelper.ReadFromByteArray(byteArray);
            MoveIndex = byteArray.ReadByte();

        }
    }

    public class BattleSnapshot
    {
        public string NewText;
        public int TextColor;
        public PokemonSnapshotSequence Pokemons;

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteUTF(NewText);
            byteArray.WriteInt(TextColor);
            Pokemons.WriteToByteArray(byteArray);

        }

        public void ReadFromByteArray(ByteArray byteArray)
        {
            NewText = byteArray.ReadUTF();
            TextColor = byteArray.ReadInt();
            Pokemons = new PokemonSnapshotSequence();
            Pokemons.ReadFromByteArray(byteArray);

        }
    }

    public class PokemonSnapshot
    {
        public int Identity;
        public string Nickname;
        public short Hp;
        public short MaxHp;
        public byte Gender;
        public byte Lv;
        public byte State;
        public bool Substituded;
        public bool Hid;

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(Identity);
            byteArray.WriteUTF(Nickname);
            byteArray.WriteShort(Hp);
            byteArray.WriteShort(MaxHp);
            byteArray.WriteByte(Gender);
            byteArray.WriteByte(Lv);
            byteArray.WriteByte(State);
            byteArray.WriteBoolean(Substituded);
            byteArray.WriteBoolean(Hid);

        }

        public void ReadFromByteArray(ByteArray byteArray)
        {
            Identity = byteArray.ReadInt();
            Nickname = byteArray.ReadUTF();
            Hp = byteArray.ReadShort();
            MaxHp = byteArray.ReadShort();
            Gender = byteArray.ReadByte();
            Lv = byteArray.ReadByte();
            State = byteArray.ReadByte();
            Substituded = byteArray.ReadBoolean();
            Hid = byteArray.ReadBoolean();

        }
    }

    public class BattleInfo
    {
        public BattleTerrain Terrain;
        public BattleMode Mode;
        public string Caption;
        public string CustomDataHash;

        public void WriteToByteArray(ByteArray byteArray)
        {
            BattleTerrainHelper.WriteToByteArray(byteArray, Terrain);
            BattleModeHelper.WriteToByteArray(byteArray, Mode);
            byteArray.WriteUTF(Caption);
            byteArray.WriteUTF(CustomDataHash);

        }

        public void ReadFromByteArray(ByteArray byteArray)
        {
            Terrain = BattleTerrainHelper.ReadFromByteArray(byteArray);
            Mode = BattleModeHelper.ReadFromByteArray(byteArray);
            Caption = byteArray.ReadUTF();
            CustomDataHash = byteArray.ReadUTF();

        }
    }

    #endregion

    #region Sequences

    public class ByteSequence
    {

        private List<byte> _elements = new List<byte>();

        public List<byte> Elements
        {
            get { return _elements; }
        }


        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(_elements.Count);

            foreach (byte elem in _elements)
            {
                byteArray.WriteByte(elem);
            }
        }

        public void ReadFromByteArray(ByteArray byteArray)
        {
            int cnt = byteArray.ReadInt();

            for (int i = 0; i < cnt; i++)
            {
                byte elem = byteArray.ReadByte();
                _elements.Add(elem);
            }
        }
    }

    public class BattleRuleSequence
    {

        private List<BattleRule> _elements = new List<BattleRule>();

        public List<BattleRule> Elements
        {
            get { return _elements; }
        }


        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(_elements.Count);

            foreach (BattleRule elem in _elements)
            {
                BattleRuleHelper.WriteToByteArray(byteArray, elem);
            }
        }

        public void ReadFromByteArray(ByteArray byteArray)
        {
            int cnt = byteArray.ReadInt();

            for (int i = 0; i < cnt; i++)
            {
                BattleRule elem = BattleRuleHelper.ReadFromByteArray(byteArray);
                _elements.Add(elem);
            }
        }
    }

    public class PokemonSnapshotSequence
    {

        private List<PokemonSnapshot> _elements = new List<PokemonSnapshot>();

        public List<PokemonSnapshot> Elements
        {
            get { return _elements; }
        }


        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteInt(_elements.Count);

            foreach (PokemonSnapshot elem in _elements)
            {
                elem.WriteToByteArray(byteArray);
            }
        }

        public void ReadFromByteArray(ByteArray byteArray)
        {
            int cnt = byteArray.ReadInt();

            for (int i = 0; i < cnt; i++)
            {
                PokemonSnapshot elem = new PokemonSnapshot();
                elem.ReadFromByteArray(byteArray);
                _elements.Add(elem);
            }
        }
    }

    #endregion
}
namespace PokemonBattle.BattleNetwork.Server
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
        
        
        public class PokemonBattleServerHelper
        {
            
            public static ByteArray LogonSuccess()
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_LogonSuccess_Hash );
                
                return byteArray;
            }
            
            public static ByteArray LogonFail(string message)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_LogonFail_Hash );
                byteArray.WriteUTF(message);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveRandomSeed(int seed)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_ReceiveRandomSeed_Hash );
                byteArray.WriteInt(seed);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveMove(PlayerMove move)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_ReceiveMove_Hash );
                move.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveTeam(byte position,string identity,ByteSequence team)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_ReceiveTeam_Hash );
                byteArray.WriteByte(position);
                    
                byteArray.WriteUTF(identity);
                    
                team.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveRules(BattleRuleSequence rules)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_ReceiveRules_Hash );
                rules.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveTieMessage(string identity,TieMessage message)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_ReceiveTieMessage_Hash );
                byteArray.WriteUTF(identity);
                    
                TieMessageHelper.WriteToByteArray(byteArray,message);
                    
                
                return byteArray;
            }
            
            public static ByteArray Exit(string identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_Exit_Hash );
                byteArray.WriteUTF(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray TimeUp(string identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_TimeUp_Hash );
                byteArray.WriteUTF(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray RegistObsever(int identity)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_RegistObsever_Hash );
                byteArray.WriteInt(identity);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveBattleInfo(BattleInfo info)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_ReceiveBattleInfo_Hash );
                info.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            
            public static ByteArray ReceiveBattleSnapshot(BattleSnapshot snapshot)
            {
                ByteArray byteArray = new ByteArray();

                byteArray.WriteInt(EnumPokemonBattleMethods.Client_ReceiveBattleSnapshot_Hash );
                snapshot.WriteToByteArray(byteArray);
                    
                
                return byteArray;
            }
            

            public static bool InterpretMessage(int sessionID ,ByteArray byteArray,IPokemonBattleServerService serverService)
            {
                byteArray.BypassHeader();
                int methodID = byteArray.ReadInt();

                switch(methodID)
                {
                
                    case EnumPokemonBattleMethods.Server_Logon_Hash:
                        return OnLogon(sessionID, byteArray, serverService);
                
                    case EnumPokemonBattleMethods.Server_ReceiveMove_Hash:
                        return OnReceiveMove(sessionID, byteArray, serverService);
                
                    case EnumPokemonBattleMethods.Server_ReceiveTeam_Hash:
                        return OnReceiveTeam(sessionID, byteArray, serverService);
                
                    case EnumPokemonBattleMethods.Server_Exit_Hash:
                        return OnExit(sessionID, byteArray, serverService);
                
                    case EnumPokemonBattleMethods.Server_TimeUp_Hash:
                        return OnTimeUp(sessionID, byteArray, serverService);
                
                    case EnumPokemonBattleMethods.Server_ReceiveTieMessage_Hash:
                        return OnReceiveTieMessage(sessionID, byteArray, serverService);
                
                    case EnumPokemonBattleMethods.Server_RegistObsever_Hash:
                        return OnRegistObsever(sessionID, byteArray, serverService);
                
                    case EnumPokemonBattleMethods.Server_ReceiveBattleInfo_Hash:
                        return OnReceiveBattleInfo(sessionID, byteArray, serverService);
                
                    case EnumPokemonBattleMethods.Server_ReceiveBattleSnapshot_Hash:
                        return OnReceiveBattleSnapshot(sessionID, byteArray, serverService);
                
                }

                byteArray.Rewind();
                return false;
            }
            
            private static bool OnLogon(int sessionID, ByteArray byteArray ,IPokemonBattleServerService serverService)
                {
                string identity = byteArray.ReadUTF();
                BattleMode modeInfo = BattleModeHelper.ReadFromByteArray(byteArray);
                string versionInfo = byteArray.ReadUTF();
                
                serverService.OnLogon(sessionID, identity, modeInfo, versionInfo);
                return true;
            }
            
            private static bool OnReceiveMove(int sessionID, ByteArray byteArray ,IPokemonBattleServerService serverService)
                {
                PlayerMove move = new PlayerMove();
		move.ReadFromByteArray(byteArray);
                
                serverService.OnReceiveMove(sessionID, move);
                return true;
            }
            
            private static bool OnReceiveTeam(int sessionID, ByteArray byteArray ,IPokemonBattleServerService serverService)
                {
                byte position = byteArray.ReadByte();
                string identity = byteArray.ReadUTF();
                ByteSequence team = new ByteSequence();
		team.ReadFromByteArray(byteArray);
                
                serverService.OnReceiveTeam(sessionID, position, identity, team);
                return true;
            }
            
            private static bool OnExit(int sessionID, ByteArray byteArray ,IPokemonBattleServerService serverService)
                {
                string identity = byteArray.ReadUTF();
                
                serverService.OnExit(sessionID, identity);
                return true;
            }
            
            private static bool OnTimeUp(int sessionID, ByteArray byteArray ,IPokemonBattleServerService serverService)
                {
                string identity = byteArray.ReadUTF();
                
                serverService.OnTimeUp(sessionID, identity);
                return true;
            }
            
            private static bool OnReceiveTieMessage(int sessionID, ByteArray byteArray ,IPokemonBattleServerService serverService)
                {
                string identity = byteArray.ReadUTF();
                TieMessage message = TieMessageHelper.ReadFromByteArray(byteArray);
                
                serverService.OnReceiveTieMessage(sessionID, identity, message);
                return true;
            }
            
            private static bool OnRegistObsever(int sessionID, ByteArray byteArray ,IPokemonBattleServerService serverService)
                {
                int identity = byteArray.ReadInt();
                
                serverService.OnRegistObsever(sessionID, identity);
                return true;
            }
            
            private static bool OnReceiveBattleInfo(int sessionID, ByteArray byteArray ,IPokemonBattleServerService serverService)
                {
                BattleInfo info = new BattleInfo();
		info.ReadFromByteArray(byteArray);
                
                serverService.OnReceiveBattleInfo(sessionID, info);
                return true;
            }
            
            private static bool OnReceiveBattleSnapshot(int sessionID, ByteArray byteArray ,IPokemonBattleServerService serverService)
                {
                BattleSnapshot snapshot = new BattleSnapshot();
		snapshot.ReadFromByteArray(byteArray);
                
                serverService.OnReceiveBattleSnapshot(sessionID, snapshot);
                return true;
            }
            
        }

        public interface IPokemonBattleServerService
        {
            
                void OnLogon(int sessionID, string identity, BattleMode modeInfo, string versionInfo);
            
                void OnReceiveMove(int sessionID, PlayerMove move);
            
                void OnReceiveTeam(int sessionID, byte position, string identity, ByteSequence team);
            
                void OnExit(int sessionID, string identity);
            
                void OnTimeUp(int sessionID, string identity);
            
                void OnReceiveTieMessage(int sessionID, string identity, TieMessage message);
            
                void OnRegistObsever(int sessionID, int identity);
            
                void OnReceiveBattleInfo(int sessionID, BattleInfo info);
            
                void OnReceiveBattleSnapshot(int sessionID, BattleSnapshot snapshot);
            
        }
        
    
    #endregion
}
    