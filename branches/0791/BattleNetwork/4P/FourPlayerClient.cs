using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using PokemonBattle.FourPlayer.Client;
using NetworkLib.Tcp;

namespace PokemonBattle.BattleNetwork
{
    public delegate void SetPositionDelegate(byte position, string player);
    public delegate void MyPositionDelegate(byte position);
    public delegate void IdentityDelegate(int identity);
    public class FourPlayerClient : NetworkClient, IProtocolInterpreter, IFourPlayerClientService
    {
        public event SetPositionDelegate OnPositionSet;
        public event IdentityDelegate OnBattleReady;
        public event MyPositionDelegate OnMyPositionSet;
        public event VoidFunctionDelegate OnServerClosed;
        public FourPlayerClient(string serverIP)
        {
            TcpNetworkStrategy strategy = new TcpNetworkStrategy();
            strategy.Port = (int)ServerPort.FourPlayerServerPort;
            strategy.Sync = true;
            strategy.ServerIP = serverIP;
            NetworkStrategy = strategy;

            _interpreter = this;
        }

        public void SetPort(int port)
        {
            (NetworkStrategy as TcpNetworkStrategy).Port = port;
        }

        public void SetPosition(byte position,string player)
        {
            Send(FourPlayerClientHelper.SetPosition(position, player));
        }

        public void StartBattle()
        {
            Send(FourPlayerClientHelper.StartBattle());
        }

        public void Close()
        {
            Send(FourPlayerClientHelper.Close());
        }

        public void Logon(int identity)
        {
            Send(FourPlayerClientHelper.Logon(identity));
        }

        #region IFourPlayerClientService 成员

        public void OnSetPosition(byte position, string player)
        {
            if (OnPositionSet != null) OnPositionSet(position, player);
        }

        public void OnStartBattle(int identity)
        {
            if (OnBattleReady != null) OnBattleReady(identity);
        }

        public void OnSetPositionSuccess(byte position)
        {
            if (OnMyPositionSet != null) OnMyPositionSet(position);
        }

        public void OnClose()
        {
            if (OnServerClosed != null) OnServerClosed();
        }

        #endregion

        #region IProtocolInterpreter 成员

        public bool InterpretMessage(int sessionID, NetworkLib.Utilities.ByteArray byteArray)
        {
            return FourPlayerClientHelper.InterpretMessage(sessionID, byteArray, this);
        }

        #endregion
    }
}
