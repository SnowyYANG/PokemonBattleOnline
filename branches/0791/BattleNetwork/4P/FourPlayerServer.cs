using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using PokemonBattle.FourPlayer.Server;
using NetworkLib.Tcp;

namespace PokemonBattle.BattleNetwork
{
    public class FourPlayerServer : NetworkServer,IProtocolInterpreter,IFourPlayerServerService
    {
        public event SetPositionDelegate OnPositionSet;

        private Dictionary<byte, string> _positions = new Dictionary<byte, string>();
        private object _locker = new object();

        public FourPlayerServer(string serverPlayer)
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = (int)ServerPort.FourPlayerServerPort;
            strategy.Sync = true;
            NetworkStrategy = strategy;

            _interpreter = this;
            OnClientDisconnected += new SessionDisconnectedDelegate(FourPlayerServer_OnClientDisconnected);

            _positions[1] = serverPlayer;
            for (byte i = 2; i < 5; i++)
            {
                _positions[i] = "";
            }
        }

        private void FourPlayerServer_OnClientDisconnected(ClientSession client)
        {
            lock (_locker)
            {
                FourPlayerSession session = client as FourPlayerSession;
                if (session.Position != 0)
                {
                    SetPosition(session.Position, "");
                }
            }
        }

        protected override ClientSession CreateClientSession(int sessionID, IReactor reactor)
        {
            return new FourPlayerSession(sessionID, reactor, Buffered);
        }

        private void SetPosition(byte position, string player)
        {
            _positions[position] = player;
            BroadCast(FourPlayerServerHelper.SetPosition(position, player));
            if (OnPositionSet != null) OnPositionSet(position, player);
        }

        public void StartBattle()
        {
            BroadCast(FourPlayerServerHelper.StartBattle(0));
        }

        #region IFourPlayerServerService 成员

        public void OnLogon(int sessionID, int identity)
        {
            lock (_locker)
            {
                for (byte i = 1; i < 5; i++)
                {
                    Send(sessionID, FourPlayerServerHelper.SetPosition(i, _positions[i]));
                }
            }
        }

        public void OnSetPosition(int sessionID, byte position, string player)
        {
            lock (_locker)
            {
                if (_positions[position] == "")
                {
                    FourPlayerSession session = GetClient(sessionID) as FourPlayerSession;
                    if (session.Position != 0)
                    {
                        SetPosition(session.Position, "");
                    }
                    SetPosition(position, player);
                    session.Position = position;
                    Send(sessionID, FourPlayerServerHelper.SetPositionSuccess(position));
                }
            }
        }

        public void OnStartBattle(int sessionID)
        {}

        public void OnClose(int sessionID)
        {
        }
        #endregion

        #region IProtocolInterpreter 成员

        public bool InterpretMessage(int sessionID, NetworkLib.Utilities.ByteArray byteArray)
        {
            return FourPlayerServerHelper.InterpretMessage(sessionID, byteArray, this);
        }

        #endregion
    }
}
