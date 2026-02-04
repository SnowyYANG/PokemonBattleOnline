using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PokemonBattleOnline.Network.Commands
{
    public class ClientInitC2S : IC2S
    {
        public string version {get;set;}

        public string name {get;set;}
        
        public string room {get;set;}

        public Seat seat {get;set;}
    }
    
    public class ClientInitS2C : IS2C
    {
        public string type => "init";
        [JsonPropertyName("id")]
        public readonly string ID;

        public ClientInitS2C(string user, User[] roomUsers)
        {
            ID = user;
            _roomUsers = roomUsers;
        }

        [JsonPropertyName("users")]
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
    public class WelcomeS2C : IS2C
    {
        public string type => "welcome";

        [JsonPropertyName("version")]
        public string ServerVersion {get;set;}
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
