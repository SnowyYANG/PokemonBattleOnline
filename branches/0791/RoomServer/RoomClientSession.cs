using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib;
using PokemonBattle.BattleRoom.Server;
namespace PokemonBattle.RoomServer
{
    internal class RoomClientSession : ClientSession
    {
        private ObserveInfo _observeInfo = new ObserveInfo();
        public ObserveInfo ObserveInfo
        {
            get { return _observeInfo; }
        }
        public RoomClientSession(int sessionID, IReactor reactor, bool buffered)
            : base(sessionID, reactor, buffered)
        {
        }
    }
}
