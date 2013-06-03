using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;

namespace PokemonBattle.BattleNetwork
{
    internal class FourPlayerSession : ClientSession
    {
        private byte _position = 0;
        public byte Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public FourPlayerSession(int sessionID, IReactor reactor, bool buffered)
            : base(sessionID, reactor, buffered)
        {
        }
    }
}
