using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network
{
    [DataContract(Namespace = PBOMarks.JSON)]
    public class User : ObservableObject
    {
        public User(string id, string name, string room, Seat seat)
        {
            _id = id;
            _name = name;
            RoomId = room;
            Seat = seat;
        }

        [DataMember(Name = "id")]
        private readonly string _id;
        public string Id
        { get { return _id; } }
        [DataMember(Name = "name")]
        private readonly string _name;
        public string Name
        { get { return _name; } }

        [DataMember(Name = "room")]
        public string RoomId;

        private Room _room;
        /// <summary>
        /// setter is only for Room class
        /// </summary>
        public Room Room
        {
            get
            {
                return _room;
            }
            internal set
            {
                if (_room != value)
                {
                    _room = value;
                    RoomId = _room == null ? null : _room.Id;
                }
            }
        }
        [DataMember(Name = "seat")]
        public Seat Seat
        { get; internal set; }
    }
}
