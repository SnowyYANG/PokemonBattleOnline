using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using PokemonBattle.BattleNetwork.Server;
using NetworkLib.Tcp;

namespace PokemonBattle.BattleNetwork
{
    public delegate bool ClientConnectedDelegate(string identity, BattleMode modeInfo, string versionInfo, out string message);
    public delegate BattleInfo RequestBattleInfoDelegate();
    public delegate void ServerTieDelegate(int identity,string player, TieMessage message);
    public class PokemonBattleServer : NetworkServer,IProtocolInterpreter ,IPokemonBattleServerService
    {
        #region Events

        public event ClientConnectedDelegate OnBattleClientConnected;
        private bool VerifyClient(string identity, BattleMode modeInfo, string versionInfo, out string message)
        {
            if (OnBattleClientConnected != null)
            {
                return OnBattleClientConnected(identity, modeInfo, versionInfo, out message);
            }
            message = "";
            return true;
        }

        public event SetMoveDelegate OnSetMove;
        public event SetTeamDelegate OnSetTeam;
        public event MessageDelegate OnPlayerExit;
        public event ServerTieDelegate OnTie;
        public event RequestBattleInfoDelegate OnRequestBattleInfo;
        private BattleInfo HandleRequestBattleInfoEvent()
        {
            if (OnRequestBattleInfo != null) return OnRequestBattleInfo();
            return null;
        }
        #endregion

        private Dictionary<int, string> _playerList = new Dictionary<int, string>();
        private List<int> _observerList = new List<int>();

        public PokemonBattleServer()
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = (int)ServerPort.PokemonBattleServerPort;
            strategy.Sync = true;

            NetworkStrategy = strategy;
            _interpreter = this;

            OnClientDisconnected += new SessionDisconnectedDelegate(PokemonBattleServer_OnClientDisconnected);

        }

        private void PokemonBattleServer_OnClientDisconnected(ClientSession client)
        {
            if (_playerList.ContainsKey(client.SessionID))
            {
                OnExit(client.SessionID, _playerList[client.SessionID]);
            }
            else if (_observerList.Contains(client.SessionID))
            {
                _observerList.Remove(client.SessionID);
            }
        }

        public void SendRandomSeed(int seed)
        {
            foreach (int key in _playerList.Keys)
            {
                Send(key, PokemonBattleServerHelper.ReceiveRandomSeed(seed));
            }
        }

        public void SendRules(BattleRuleSequence rules)
        {
            foreach (int key in _playerList.Keys)
            {
                Send(key, PokemonBattleServerHelper.ReceiveRules(rules));
            }
        }

        public void SendMove(PlayerMove move)
        {
            foreach (int key in _playerList.Keys)
            {
                Send(key, PokemonBattleServerHelper.ReceiveMove(move));
            }
        }

        public void SendTeam(byte position, string identity, ByteSequence team)
        {
            foreach (int key in _playerList.Keys)
            {
                Send(key, PokemonBattleServerHelper.ReceiveTeam(position, identity, team));
            }
        }

        public void Exit(string identity)
        {
            foreach (int key in _playerList.Keys)
            {
                Send(key, PokemonBattleServerHelper.Exit(identity));
            }
        }

        public void Tie(string identity, TieMessage message)
        {
            foreach (int key in _playerList.Keys)
            {
                Send(key, PokemonBattleServerHelper.ReceiveTieMessage(identity, message));
            }
        }
        public void TieExcept(int sessionID, string identity, TieMessage message)
        {
            foreach (int key in _playerList.Keys)
            {
                if (key == sessionID) continue;
                Send(key, PokemonBattleServerHelper.ReceiveTieMessage(identity, message));
            }
        }
        public void TieRequestFail(int sessionID)
        {
            Send(sessionID, PokemonBattleServerHelper.ReceiveTieMessage("", TieMessage.Fail));
        }

        public void SendBattleSnapshot(BattleSnapshot snapshot)
        {
            foreach (int identity in ObserverList)
            {
                Send(identity, PokemonBattleServerHelper.ReceiveBattleSnapshot(snapshot));
            }
        }

        public void SendBattleInfo(BattleInfo info)
        {
            foreach (int identity in ObserverList)
            {
                Send(identity, PokemonBattleServerHelper.ReceiveBattleInfo(info));
            }
        }

        private List<int> ObserverList
        {
            get
            {
                return new List<int>(_observerList);
            }
        }

        #region IProtocolInterpreter 成员

        public bool InterpretMessage(int sessionID, NetworkLib.Utilities.ByteArray byteArray)
        {
            return PokemonBattleServerHelper.InterpretMessage(sessionID, byteArray, this);
        }

        #endregion

        #region IPokemonBattleServerService 成员

        public void OnLogon(int sessionID, string identity, BattleMode modeInfo, string versionInfo)
        {
            string message;
            if (VerifyClient(identity, modeInfo, versionInfo, out message))
            {
                Send(sessionID, PokemonBattleServerHelper.LogonSuccess());
                _playerList[sessionID] = identity;
            }
            else
            {
                Send(sessionID, PokemonBattleServerHelper.LogonFail(message));
                Disconnect(sessionID);
            }
        }

        public void OnReceiveMove(int sessionID, PlayerMove move)
        {
            if (OnSetMove != null)
            {
                OnSetMove(move);
            }
        }

        public void OnReceiveTeam(int sessionID, byte position, string identity, ByteSequence team)
        {
            if (OnSetTeam != null)
            {
                OnSetTeam(position, identity, team);
            }
        }

        public void OnExit(int sessionID, string identity)
        {
            if (OnPlayerExit != null) OnPlayerExit(identity);
        }

        public void OnReceiveTieMessage(int sessionID, string identity, TieMessage message)
        {
            if (OnTie != null) OnTie(sessionID, identity, message);
        }


        public void OnRegistObsever(int sessionID, int identity)
        {
            BattleInfo info = HandleRequestBattleInfoEvent();
            if (info != null)
            {
                Send(sessionID, PokemonBattleServerHelper.ReceiveBattleInfo(info));
            }
            _observerList.Add(sessionID);
        }

        public void OnReceiveBattleInfo(int sessionID, BattleInfo info)
        {}
        public void OnReceiveBattleSnapshot(int sessionID, BattleSnapshot snapshot)
        { }
        public void OnTimeUp(int sessionID, string identity)
        {}
        #endregion
    }
}
