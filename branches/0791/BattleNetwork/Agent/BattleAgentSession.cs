using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;

namespace PokemonBattle.BattleNetwork
{
    public class BattleAgentSession : ClientSession
    {
        private int _agentID = -1;
        public int AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
        }
        public BattleAgentSession(int sessionID, IReactor reactor, bool buffered)
            : base(sessionID, reactor, buffered)
        {
        }
    }
}
