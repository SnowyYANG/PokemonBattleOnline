using System;
using System.Collections.Generic;
using System.Text;
using PokemonBattle.BattleNetwork;
using PokemonBattle.BattleNetwork.Server;
using NetworkLib;
using NetworkLib.Tcp;
using NetworkLib.Utilities;

namespace PokemonBattle.BattleNetwork
{
    public class BattleAgentServer : NetworkServer, IProtocolInterpreter, IPokemonBattleServerService
    {
        private int _battleIdentityBase = -1;
        private Dictionary<int, BattleAgent> _agents = new Dictionary<int, BattleAgent>();

        public BattleAgentServer()
        {

            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = (int)ServerPort.BattleAgentServerPort;
            strategy.Sync = true;

            NetworkStrategy = strategy;
            _interpreter = this;
            UpdateInterval = 500;
            OnLogicUpdate += new VoidFunctionDelegate(BattleAgentServer_OnLogicUpdate);
            OnClientDisconnected += new SessionDisconnectedDelegate(BattleAgentServer_OnClientDisconnected);
        }

        void BattleAgentServer_OnClientDisconnected(ClientSession client)
        {
            BattleAgentSession session = client as BattleAgentSession;

            if (session.AgentID != -1)
            {
                BattleAgent agent = GetAgent(session.AgentID);
                if (agent != null && !agent.BattleEnd)
                {
                    agent.PlayerExit(client.SessionID);
                }
            }
        }

        void BattleAgentServer_OnLogicUpdate()
        {
            List<int> keys = new List<int>(_agents.Keys);
            foreach (int key in keys)
            {
                if (_agents[key].BattleEnd)
                {
                    _agents.Remove(key);
                    Logger.LogInfo("Remove battle agent, ID : {0}", key);
                }
            }
        }

        protected override ClientSession CreateClientSession(int sessionID, IReactor reactor)
        {
            return new BattleAgentSession(sessionID, reactor, Buffered);
        }

        public int AddBattle(BattleMode mode,BattleRuleSequence rules)
        {
            _battleIdentityBase++;
            BattleAgent agent = new BattleAgent(mode, rules.Elements,this);
            _agents[_battleIdentityBase] = agent;

            Logger.LogInfo("Add battle agent, ID : {0}", _battleIdentityBase);
            return _battleIdentityBase;
        }

        internal void SendTeam(int sessionID, byte position, string identity, ByteSequence byteSequence)
        {
            Send(sessionID, PokemonBattleServerHelper.ReceiveTeam(position, identity, byteSequence));
        }

        internal void SendMove(int sessionID, PlayerMove move)
        {
            Send(sessionID, PokemonBattleServerHelper.ReceiveMove(move));
        }

        internal void SendExitMessage(int sessionID, string identity)
        {
            Send(sessionID, PokemonBattleServerHelper.Exit(identity));
        }

        internal void SendTieMessage(int sessionID, string identity, TieMessage message)
        {
            Send(sessionID, PokemonBattleServerHelper.ReceiveTieMessage(identity, message));
        }

        internal void SendBattleData(int sessionID, List<BattleRule> rules, int randomSeed)
        {
            BattleRuleSequence ruleSequence = new BattleRuleSequence();
            ruleSequence.Elements.AddRange(rules);
            Send(sessionID, PokemonBattleServerHelper.ReceiveRules(ruleSequence));
            Send(sessionID, PokemonBattleServerHelper.ReceiveRandomSeed(randomSeed));
        }

        internal void SendBattleSnapshot(int sessionID, BattleSnapshot snapshot)
        {
            Send(sessionID, PokemonBattleServerHelper.ReceiveBattleSnapshot(snapshot));
        }

        internal void SendBattleInfo(int sessionID, BattleInfo info)
        {
            Send(sessionID, PokemonBattleServerHelper.ReceiveBattleInfo(info));
        }

        internal void RegistObserver(int sessionID)
        {
            Send(sessionID, PokemonBattleServerHelper.RegistObsever(0));
        }

        internal void PlayerTimeUp(int sessionID, string identity)
        {
            Send(sessionID, PokemonBattleServerHelper.TimeUp(identity));
        }

        private BattleAgent GetAgent(int agentID)
        {
            BattleAgent agent;
            bool contains = _agents.TryGetValue(agentID, out agent);
            if (contains) return agent;
            return null;
        }

        private BattleAgent GetClientAgent(int sessionID)
        {
            ClientSession client = GetClient(sessionID);
            if (client == null || (client as BattleAgentSession).AgentID == -1) return null;
            return GetAgent((client as BattleAgentSession).AgentID);
        }

        #region IPokemonBattleServerService 成员

        public void OnLogon(int sessionID, string identity, BattleMode modeInfo, string versionInfo)
        {
            int agentId;
            BattleAgent agent = null;
            if (int.TryParse(identity, out agentId))
            {
                agent = GetAgent(agentId);
            }
            if (agent != null)
            {
                if (!agent.BattleEnd)
                {
                    (GetClient(sessionID) as BattleAgentSession).AgentID = agentId;
                    agent.UserLogon(sessionID, identity);
                    Send(sessionID, PokemonBattleServerHelper.LogonSuccess());
                }
                else
                {
                    Send(sessionID, PokemonBattleServerHelper.LogonFail("对手已退出"));
                    Disconnect(sessionID);
                }
            }
            else
            {
                Send(sessionID, PokemonBattleServerHelper.LogonFail("无法找到对应的对战代理"));
                Disconnect(sessionID);
            }
        }

        public void OnReceiveMove(int sessionID, PlayerMove move)
        {
            BattleAgent agent = GetClientAgent(sessionID);
            if (agent != null)
            {
                agent.ReceiveMove(sessionID, move);
            }
        }

        public void OnReceiveTeam(int sessionID, byte position, string identity, ByteSequence team)
        {
            BattleAgent agent = GetClientAgent(sessionID);
            if (agent != null)
            {
                agent.ReceiveTeam(sessionID, position, identity, team);
            }
        }

        public void OnExit(int sessionID, string identity)
        {
            Disconnect(sessionID);
        }

        public void OnReceiveTieMessage(int sessionID, string identity, TieMessage message)
        {
            BattleAgent agent = GetClientAgent(sessionID);
            if (agent != null)
            {
                agent.ReceiveTieMessage(sessionID, message);
            }
        }

        public void OnRegistObsever(int sessionID, int identity)
        {
            BattleAgent agent = GetAgent(identity);
            if (agent != null && !agent.BattleEnd)
            {
                agent.RegistObserver(sessionID);
            }
        }

        public void OnReceiveBattleInfo(int sessionID, BattleInfo info)
        {
            BattleAgent agent = GetClientAgent(sessionID);
            if (agent != null)
            {
                agent.ReceiveBattleInfo(info);
            }
        }

        public void OnReceiveBattleSnapshot(int sessionID, BattleSnapshot snapshot)
        {
            BattleAgent agent = GetClientAgent(sessionID);
            if (agent != null)
            {
                agent.ReceiveBattleSnapshot(snapshot);
            }
        }

        public void OnTimeUp(int sessionID, string identity)
        {
            BattleAgent agent = GetClientAgent(sessionID);
            if (agent != null)
            {
                agent.PlayerTimeUp(sessionID, identity);
            }
        }
        #endregion

        #region IProtocolInterpreter 成员

        public bool InterpretMessage(int sessionID, ByteArray byteArray)
        {
            return PokemonBattleServerHelper.InterpretMessage(sessionID, byteArray, this);
        }

        #endregion
    }
}
