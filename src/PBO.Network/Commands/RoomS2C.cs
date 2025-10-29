using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.Commands
{
    [DataContract(Name = "room", Namespace = PBOMarks.JSON)]
    public class RoomS2C : IS2C
    {
        public static RoomS2C ChangeBattling(string id)
        {
            return new RoomS2C() { Id = id, Battling = true };
        }

        [DataMember(Name = "id")]
        private string Id;
        [DataMember(Name = "b_", EmitDefaultValue = false)]
        private GameSettings Settings;
        [DataMember(Name = "battling", EmitDefaultValue = false)]
        private bool Battling;

        private RoomS2C()
        {
        }

        void IS2C.Execute(PboClient client)
        {
            if (Battling)
            {
                var room = client.RoomController.Room;
                room.Battling = !room.Battling;
            }
        }
    }
}
