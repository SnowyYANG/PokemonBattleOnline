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
        public static UserS2C AddUser(string id, string name, string room, Seat seat)
        {
            return new UserS2C() { Id = id, Name = name, Seat = seat };
        }
        public static UserS2C RemoveUser(string id)
        {
            return new UserS2C() { Id = id };
        }

        private UserS2C()
        {
        }

        [DataMember(Name = "id")]
        string Id;
        [DataMember(Name = "name", EmitDefaultValue = false)]
        string Name;
        [DataMember(Name = "seat")]
        Seat Seat;

        void IS2C.Execute(PboClient client)
        {
            if (Name == null) client.RoomController.Room.RemoveUser(Id);
            else client.RoomController.Room.AddUser(Id, Name, Seat);
        }
    }
}
