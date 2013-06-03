using System;
using System.Collections.Generic;
using System.Text;
using PokemonBattle.BattleNetwork.Server;
using System.Threading;
namespace PokemonBattle.BattleNetwork
{
    internal class BattleAgent
    {
        #region Varibles

        private BattleAgentServer _server;
        private BattleMode _battleMode;
        private List<BattleRule> _rules;
        private int _randomSeed;
        private BattleInfo _battleInfo;

        private Dictionary<int, UserInfo> _users = new Dictionary<int, UserInfo>();
        private List<int> _observers = new List<int>();
        private int observeHost;

        private int _waitUserCount;
        private object _locker = new object();

        private int _agreeTieCounter;
        private bool _tieing;
        private bool _battleEnd;
        #endregion

        public BattleAgent(BattleMode mode, List<BattleRule> rules,BattleAgentServer server)
        {
            _server = server;
            _battleMode = mode;
            _rules = rules;
            _randomSeed = new Random().Next();

            if (mode == BattleMode.Double_4P)
            {
                _waitUserCount = 4;
            }
            else
            {
                _waitUserCount = 2;
            }
        }

        public void UserLogon(int identity, string name)
        {
            if (!_users.ContainsKey(identity))
            {
                lock (_locker)
                {
                    if (_waitUserCount == 0) return;
                }
                UserInfo info = new UserInfo(name, 0, null);
                _users[identity] = info;
            }
        }

        public void ReceiveTeam(int identity, byte position, string name, ByteSequence teamData)
        {
            if (_users.ContainsKey(identity))
            {
                _users[identity].Name = name;
                _users[identity].TeamData = teamData;
                _users[identity].Position = position;
                lock (_locker)
                {
                    Interlocked.Decrement(ref _waitUserCount);
                    if (_waitUserCount == 0)
                    {
                        SendData();
                    }
                }
            }
        }

        private void SendData()
        {
            foreach (int userKey in _users.Keys)
            {
                foreach (int userKey2 in _users.Keys)
                {
                    if (userKey != userKey2)
                    {
                        _server.SendTeam(userKey, _users[userKey2].Position, _users[userKey2].Name, _users[userKey2].TeamData);
                    }
                }
            }

            foreach (int userKey in _users.Keys)
            {
                _server.SendBattleData(userKey, _rules, _randomSeed);
            }
            observeHost = new List<int>(_users.Keys)[0];
            _server.RegistObserver(observeHost);
        }

        public void ReceiveMove(int identity, PlayerMove move)
        {
            if (_users.ContainsKey(identity))
            {
                foreach (int userKey in _users.Keys)
                {
                    _server.SendMove(userKey, move);
                }
            }
        }

        public void PlayerExit(int identity)
        {
            if (_users.ContainsKey(identity))
            {
                foreach (int userKey in _users.Keys)
                {
                    if (identity != userKey)
                    {
                        _server.SendExitMessage(userKey, _users[identity].Name);
                    }
                }
                List<int> observers = GetObserverList();
                if (identity == observeHost)
                {
                    foreach (int ob in observers)
                    {
                        _server.SendExitMessage(ob, _users[identity].Name);
                    }
                }
                _battleEnd = true;
            }
        }

        public void ReceiveTieMessage(int identity, TieMessage message)
        {
            if (!_users.ContainsKey(identity)) return;
            if (message == TieMessage.TieRequest)
            {
                if (_tieing)
                {
                    _server.SendTieMessage(identity, "", TieMessage.Fail);
                    return;
                }
                else
                {
                    _tieing = true;
                }
            }
            if (!_tieing) return;
            if (message == TieMessage.AgreeTie)
            {
                _agreeTieCounter++;
                if (_agreeTieCounter + 1 != _users.Count) return; 
            }
            else if (message == TieMessage.RefuseTie)
            {
                _tieing = false;
                _agreeTieCounter = 0;
            }
            foreach (int userKey in _users.Keys)
            {
                if (message != TieMessage.AgreeTie && userKey == identity) continue;
                _server.SendTieMessage(userKey, _users[identity].Name, message);
            }
        }

        public void PlayerTimeUp(int identity, string player)
        {
            if (_users.ContainsKey(identity))
            {
                foreach (int userKey in _users.Keys)
                {
                    _server.PlayerTimeUp(userKey, player);
                }
            }
        }

        public void RegistObserver(int identity)
        {
            _observers.Add(identity);
            if (_battleInfo != null) _server.SendBattleInfo(identity, _battleInfo);
        }

        public void ReceiveBattleInfo(BattleInfo info)
        {
            _battleInfo = info;
            List<int> observers = GetObserverList();
            foreach (int identity in observers)
            {
                _server.SendBattleInfo(identity, info);
            }
        }

        public void ReceiveBattleSnapshot(BattleSnapshot snapshot)
        {
            List<int> observers = GetObserverList();
            foreach (int identity in observers)
            {
                _server.SendBattleSnapshot(identity, snapshot);
            }
        }

        private List<int> GetObserverList()
        {
            return new List<int>(_observers);
        }

        public bool BattleEnd
        {
            get { return _battleEnd; }
        }

        #region Embeded Class
        private class UserInfo
        {
            private string _name;
            private byte _position;
            private ByteSequence _teamData;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            public byte Position
            {
                get { return _position; }
                set { _position = value; }
            }
            public ByteSequence TeamData
            {
                get { return _teamData; }
                set { _teamData = value; }
            }

            public UserInfo(string name, byte position, ByteSequence teamData)
            {
                _name = name;
                _position = position;
                _teamData = teamData;
            }
        }
        #endregion
    }
}
