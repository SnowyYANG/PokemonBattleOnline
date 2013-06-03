using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBattle.BattleNetwork
{
    public class FourPlayerAgent
    {
        #region Varibles

        private FourPlayerAgentServer _server;
        private int _identity;

        private Dictionary<int, byte> _players = new Dictionary<int, byte>();
        private Dictionary<byte, string> _positions = new Dictionary<byte, string>();
        private object _locker = new object();

        private bool _closed;
        #endregion

        public FourPlayerAgent(int identity, FourPlayerAgentServer server, string hostName)
        {
            _identity = identity;
            _server = server;
            _positions[1] = hostName;
            for (byte i = 2; i < 5; i++)
            {
                _positions[i] = "";
            }
        }

        internal void PlayerLogon(int identity)
        {
            _players[identity] = 0;
            Dictionary<byte, string> pos = Positions;
            foreach (byte key in pos.Keys)
            {
                _server.SetPosition(identity, key, pos[key]);
            }
        }

        internal void SetPosition(int identity, byte position, string player)
        {
            lock (_locker)
            {
                if (_positions[position] == "")
                {
                    SetPosition(position, player);
                    if (_players[identity] != 0)
                    {
                        SetPosition(_players[identity], "");
                    }
                    else
                    {
                        _server.UpdateRoom(_identity, GetPlayerCount());
                    }
                    _players[identity] = position;
                    _server.SetPositionSuccess(identity, position);
                }
            }
        }

        internal void SetPosition(byte position, string player)
        {
            _positions[position] = player;
            foreach (int identity in PlayerList)
            {
                _server.SetPosition(identity, position, player);
            }
        }

        internal void StartBattle(int battleIdentity)
        {
            foreach (int identity in PlayerList)
            {
                _server.StartBattle(identity, battleIdentity);
            }
            _closed = true;
        }

        internal void PlayerExit(int identity)
        {
            if (_players[identity] != 0)
            {
                SetPosition(_players[identity], "");
                _players.Remove(identity);
                _server.UpdateRoom(_identity, GetPlayerCount());
            }
        }

        internal void Close()
        {
            _closed = true;

            foreach (int identity in PlayerList)
            {
                _server.Close(identity);
            }
        }

        public byte GetPlayerCount()
        {
            byte count = 4;
            Dictionary<byte, string> pos = Positions;
            foreach (byte key in pos.Keys)
            {
                if (pos[key] == "") count--;
            }
            
            return count;
        }
        public int AgentID
        {
            get { return _identity; }
        }
        public string HostName
        {
            get { return _positions[1]; }
        }

        private List<int> PlayerList
        {
            get { return new List<int>(_players.Keys); }
        }
        private Dictionary<byte, string> Positions
        {
            get
            {
                return new Dictionary<byte, string>(_positions);
            }
        }

        public bool Closed
        {
            get { return _closed; }
        }

    }
}
