using System;
using System.Collections.Generic;
using System.Text;
using PokemonBattle.BattleRoom.Server;

namespace PokemonBattle.RoomServer
{
    public class RoomSetting
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _welcomeMessage;

        public string WelcomeMessage
        {
            get { return _welcomeMessage; }
            set { _welcomeMessage = value; }
        }

        private byte _maxUser = 50;

        public byte MaxUser
        {
            get { return _maxUser; }
            set { _maxUser = value; }
        }

        private bool _logonList = true;

        public bool LogonList
        {
            get { return _logonList; }
            set { _logonList = value; }
        }

        private RoomBattleSetting _battleSetting = new RoomBattleSetting();
        public RoomBattleSetting BattleSetting
        {
            get { return _battleSetting; }
        }

    }
}
