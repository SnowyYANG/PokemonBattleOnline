using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using PokemonBattle.BattleNetwork.Client;
using NetworkLib.Tcp;

namespace PokemonBattle.BattleNetwork
{

    public delegate void SetMoveDelegate(PlayerMove move);
    public delegate void SetTeamDelegate(byte position, string identity, ByteSequence team);
    public delegate void SetRulesDelegate(BattleRuleSequence rules);
    public delegate void TieDelegate(string identity, TieMessage message);
    public delegate void SetRandomSeedDelegate(int seed);
    public delegate void MessageDelegate(string message);
    public delegate void BattleInfoDelegate(BattleInfo info);
    public delegate void SnapshotDelegate(BattleSnapshot snapshot);
    public class PokemonBattleClient : NetworkClient, IProtocolInterpreter, IPokemonBattleClientService
    {
        #region Events
        public event SetMoveDelegate OnSetMove;
        public event SetTeamDelegate OnSetTeam;
        public event MessageDelegate OnPlayerExit;
        public event TieDelegate OnTie;
        public event SetRandomSeedDelegate OnSetSeed;
        public event SetRulesDelegate OnSetRules;

        public event BattleInfoDelegate OnSetBattleInfo;
        public event SnapshotDelegate OnSetSnapshot;
        public event VoidFunctionDelegate OnRegistObserver;

        public event VoidFunctionDelegate OnLogoned;
        public event MessageDelegate OnLogonFailed;
        public event MessageDelegate OnPlayerTimeUp;
        #endregion

        public PokemonBattleClient(string serverIP)
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.ServerIP = serverIP;
            strategy.Port = (int)ServerPort.PokemonBattleServerPort;
            strategy.Sync = true;

            NetworkStrategy = strategy;
            _interpreter = this;
        }

        public void SetPort(int port)
        {
            (NetworkStrategy as TcpNetworkStrategy).Port = port;
        }

        public void SendMove(PlayerMove move)
        {
            Send(PokemonBattleClientHelper.ReceiveMove(move));
        }

        public void SendTeam(byte position, string identity, ByteSequence team)
        {
            Send(PokemonBattleClientHelper.ReceiveTeam(position, identity, team));
        }

        public void Exit(string identity)
        {
            Send(PokemonBattleClientHelper.Exit(identity));
        }

        public void Tie(string identity, TieMessage message)
        {
            Send(PokemonBattleClientHelper.ReceiveTieMessage(identity, message));
        }

        public void Logon(string identity, BattleMode mode, string versionInfo)
        {
            Send(PokemonBattleClientHelper.Logon(identity,mode,versionInfo));
        }


        public void RegistObserver(int identity)
        {
            Send(PokemonBattleClientHelper.RegistObsever(identity));
        }

        public void SendBattleInfo(BattleInfo info)
        {
            Send(PokemonBattleClientHelper.ReceiveBattleInfo(info));
        }

        public void SendBattleSnapshot(BattleSnapshot snapshot)
        {
            Send(PokemonBattleClientHelper.ReceiveBattleSnapshot(snapshot));
        }

        public void TimeUp(string identity)
        {
            Send(PokemonBattleClientHelper.TimeUp(identity));
        }

        #region IPokemonBattleClientService 成员

        public void OnLogonSuccess()
        {
            if (OnLogoned != null) OnLogoned();
        }

        public void OnLogonFail(string message)
        {
            if (OnLogonFailed != null) OnLogonFailed(message);
        }

        public void OnReceiveRandomSeed(int seed)
        {
            if (OnSetSeed != null) OnSetSeed(seed);
        }

        public void OnReceiveMove(PlayerMove move)
        {
            if (OnSetMove != null)
            {
                OnSetMove(move);
            }
        }

        public void OnReceiveTeam(byte position, string identity, ByteSequence team)
        {
            if (OnSetTeam != null) OnSetTeam(position, identity, team);
        }

        public void OnReceiveRules(BattleRuleSequence rules)
        {
            if (OnSetRules != null) OnSetRules(rules);
        }

        public void OnExit(string identity)
        {
            if (OnPlayerExit != null) OnPlayerExit(identity);
        }

        public void OnReceiveTieMessage(string identity, TieMessage message)
        {
            if (OnTie != null) OnTie(identity, message);
        }


        public void OnRegistObsever(int identity)
        {
            if (OnRegistObserver != null) OnRegistObserver();
        }

        public void OnReceiveBattleInfo(BattleInfo info)
        {
            if (OnSetBattleInfo != null) OnSetBattleInfo(info);
        }

        public void OnReceiveBattleSnapshot(BattleSnapshot snapshot)
        {
            if (OnSetSnapshot != null) OnSetSnapshot(snapshot);
        }

        public void OnTimeUp(string identity)
        {
            if (OnPlayerTimeUp != null) OnPlayerTimeUp(identity);
        }
        #endregion

        #region IProtocolInterpreter 成员

        public bool InterpretMessage(int sessionID, NetworkLib.Utilities.ByteArray byteArray)
        {
            return PokemonBattleClientHelper.InterpretMessage(sessionID, byteArray, this);
        }

        #endregion

    }
}
