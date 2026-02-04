using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.Commands
{
    [DataContract(Name = "init", Namespace = PBOMarks.JSON)]
    public class ClientInitC2S : IC2S
    {
        [DataMember]
        public string version;

        [DataMember]
        public string name;
        
        [DataMember]
        public string room;

        [DataMember]
        public Seat seat;
    }
    
    [DataContract(Name = "init", Namespace = PBOMarks.JSON)]
    public class ClientInitS2C : IS2C
    {
        [DataMember(Name = "id")]
        public readonly string ID;

        public ClientInitS2C(string user, User[] roomUsers)
        {
            ID = user;
            _roomUsers = roomUsers;
        }

        [DataMember(Name = "users")]
        private readonly User[] _roomUsers;
        public IEnumerable<User> RoomUsers
        { get { return _roomUsers; } }


        void IS2C.Execute(PboClient client)
        {
            client.MyId = ID;
            client.RoomController = new RoomController(client, new Room(client.MyRoomId));
            foreach (var user in _roomUsers)
            {
                client.RoomController.Room.AddUser(user.Name, user.Seat);
            }
            client.inited = true;
        }
    }
    [DataContract(Name = "welcome", Namespace = PBOMarks.JSON)]
    public class WelcomeS2C : IS2C
    {
        [DataMember]
        public string ServerVersion;

        public WelcomeS2C(string version)
        {
            ServerVersion = "net10-" + version;
        }

        void IS2C.Execute(PboClient client)
        {
            //todo
        }
    }
}
