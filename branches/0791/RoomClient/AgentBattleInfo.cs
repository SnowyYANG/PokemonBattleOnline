using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBattle.RoomClient
{
    public class AgentBattleInfo
    {

        private string _serverAddress;

        public string ServerAddress
        {
            get { return _serverAddress; }
            set { _serverAddress = value; }
        }

        private int _agentID;

        public int AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
        }

        private BattleNetwork.BattleMode _battleMode;

        public BattleNetwork.BattleMode BattleMode
        {
            get { return _battleMode; }
            set { _battleMode = value; }
        }

        private byte _position;

        public byte Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private int _moveInterval;

        public int MoveInterval
        {
            get { return _moveInterval; }
            set { _moveInterval = value; }
        }
        
    }
}
