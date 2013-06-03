using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBattle.RoomServer
{
    public class RoomCodeSetting
    {

        private bool _useRoomCoder;
        public bool UseRoomCoder
        {
            get { return _useRoomCoder; }
            set { _useRoomCoder = value; }
        }

        private string _entranceClass;
        public string EntranceClass
        {
            get { return _entranceClass; }
            set { _entranceClass = value; }
        }

        private string _roomCodePath;
        public string RoomCodePath
        {
            get { return _roomCodePath; }
            set { _roomCodePath = value; }
        }

        private string _language;
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        private int _source;
        public int Source
        {
            get { return _source; }
            set { _source = value; }
        }

        private string _references;
        public string References
        {
            get { return _references; }
            set { _references = value; }
        }

        private List<string> _files = new List<string>();
        public List<string> Files
        {
            get { return _files; }
            set { _files = value; }
        }

    }
}
