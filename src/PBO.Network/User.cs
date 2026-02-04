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
        public User(string name, string room, Seat seat)
        {
            _name = name;
            RoomId = room;
            Seat = seat;
        }

        [DataMember(Name = "name")]
        private readonly string _name;
        public string Name
        { get { return _name; } }

        [DataMember(Name = "room")]
        public string RoomId;

        [DataMember(Name = "seat")]
        public Seat Seat
        { get; internal set; }
    }
}
