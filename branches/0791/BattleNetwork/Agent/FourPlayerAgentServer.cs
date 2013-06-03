using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using PokemonBattle.FourPlayer.Server;
using NetworkLib.Tcp;
namespace PokemonBattle.BattleNetwork
{
    public delegate void UpdateCountDelegate(int identity,byte count);
    public delegate void RemoveDelegate(int identity);
    public class FourPlayerAgentServer :NetworkServer,IProtocolInterpreter,IFourPlayerServerService
    {
        public event UpdateCountDelegate OnUpdateRoom;
        public event RemoveDelegate OnRemoveRoom;
        public event IntFunctionDelegate OnAddBattle;
        private int HandleAddBattleEvent()
        {
            if (OnAddBattle != null) return OnAddBattle();
            return -1;
        }

        private int _agentBase = -1;
        private Dictionary<int, FourPlayerAgent> _agents = new Dictionary<int, FourPlayerAgent>();

        public FourPlayerAgentServer()
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = (int)ServerPort.FourPlayerAgentServerPort;
            strategy.Sync = true;
            NetworkStrategy = strategy;

            _interpreter = this;
            UpdateInterval = 500;
            OnClientDisconnected += new SessionDisconnectedDelegate(FourPlayerAgentServer_OnClientDisconnected);
            OnLogicUpdate += new VoidFunctionDelegate(FourPlayerAgentServer_OnLogicUpdate);
        }

        void FourPlayerAgentServer_OnLogicUpdate()
        {
            Dictionary<int, FourPlayerAgent> agents = new Dictionary<int, FourPlayerAgent>(_agents);
            foreach (int key in agents.Keys)
            {
                if (agents[key].Closed)
                {
                    _agents.Remove(key);
                    if (OnRemoveRoom != null) OnRemoveRoom(key);
                } 
            }
        }

        void FourPlayerAgentServer_OnClientDisconnected(ClientSession client)
        {
            BattleAgentSession session = client as BattleAgentSession;
            if (session.AgentID == -1) return;
            FourPlayerAgent agent = GetAgent(session.AgentID);
            if (agent != null && !agent.Closed)
            {
                agent.PlayerExit(client.SessionID);
            }
        }

        protected override ClientSession CreateClientSession(int sessionID, IReactor reactor)
        {
            return new BattleAgentSession(sessionID, reactor, Buffered);
        }

        public int AddAgent(string hostName)
        {
            _agentBase++;
            FourPlayerAgent agent = new FourPlayerAgent(_agentBase, this, hostName);
            _agents[_agentBase] = agent;
            return _agentBase;
        }

        internal void SetPosition(int sessionID, byte position, string player)
        {
            Send(sessionID, FourPlayerServerHelper.SetPosition(position, player));
        }

        internal void StartBattle(int sessionID,int identity)
        {
            Send(sessionID, FourPlayerServerHelper.StartBattle(identity));
        }

        internal void UpdateRoom(int identity, byte count)
        {
            if (OnUpdateRoom != null) OnUpdateRoom(identity, count);
        }

        internal void SetPositionSuccess(int sessionID,byte position)
        {
            Send(sessionID, FourPlayerServerHelper.SetPositionSuccess(position));
        }

        internal void Close(int identity)
        {
            Send(identity, FourPlayerServerHelper.Close());
        }

        #region IFourPlayerServerService 成员

        public void OnLogon(int sessionID, int identity)
        {
            BattleAgentSession session = GetClient(sessionID) as BattleAgentSession;
            FourPlayerAgent agent = GetAgent(identity);
            if (agent != null && !agent.Closed)
            {
                session.AgentID = identity;
                agent.PlayerLogon(sessionID);
            }
        }

        public void OnSetPosition(int sessionID, byte position, string player)
        {
            FourPlayerAgent agent = GetClientAgent(sessionID);
            if (agent != null && !agent.Closed)
            {
                agent.SetPosition(sessionID, position, player);
            }
        }

        public void OnStartBattle(int sessionID)
        {
            FourPlayerAgent agent = GetClientAgent(sessionID);
            if (agent != null)
            {
                int battleIdentity = HandleAddBattleEvent();
                agent.StartBattle(battleIdentity);
            }
        }

        public void OnClose(int sessionID)
        {
            FourPlayerAgent agent = GetClientAgent(sessionID);
            if (agent != null)
            {
                agent.Close();
            }

        }

        #endregion

        private FourPlayerAgent GetAgent(int agentID)
        {
            FourPlayerAgent agent;
            bool contains = _agents.TryGetValue(agentID, out agent);
            if (contains) return agent;
            return null;
        }

        private FourPlayerAgent GetClientAgent(int sessionID)
        {
            ClientSession client = GetClient(sessionID);
            if (client == null || (client as BattleAgentSession).AgentID == -1) return null;
            return GetAgent((client as BattleAgentSession).AgentID);
        }

        public List<FourPlayerAgent> GetAgents()
        {
            return new List<FourPlayerAgent>(_agents.Values);
        }

        #region IProtocolInterpreter 成员

        public bool InterpretMessage(int sessionID, NetworkLib.Utilities.ByteArray byteArray)
        {
            return FourPlayerServerHelper.InterpretMessage(sessionID, byteArray, this);
        }

        #endregion
    }
}
