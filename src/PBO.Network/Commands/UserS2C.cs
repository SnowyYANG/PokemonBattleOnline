using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.Commands
{
    [DataContract(Namespace = PBOMarks.JSON)]
    public class UserS2C : IS2C
    {
        public static UserS2C AddUser(string name, string room, Seat seat)
        {
            return new UserS2C() { Name = name, Room = room, Seat = seat };
        }
        public static UserS2C RemoveUser(string name)
        {
            return new UserS2C() { Name = name };
        }

        private UserS2C()
        {
        }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        string Name;
        [DataMember(Name = "room", EmitDefaultValue = false)]
        string Room;
        [DataMember(Name = "seat")]
        Seat Seat;

        void IS2C.Execute(PboClient client)
        {
            if (Name == null) client.RoomController.Room.RemoveUser(Name);
            else client.RoomController.Room.AddUser(Name, Seat);
        }
    }
}
