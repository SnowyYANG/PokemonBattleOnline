using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.C2Ss
{
  [DataContract(Namespace = PBOMarks.JSON)]
  internal class SetSeatC2S : Commands.SetSeatC2S, IC2SE
  {
    public void Execute(ServerUser su)
    {
      var user = su.User;
      var server = su.Server;
      var fr = user.Room;
      if (fr == null)
      {
        var room = GameSettings == null ? server.GetRoom(Room) : server.AddRoom(GameSettings);
        if (room != null && room.Room.IsValidSeat(Seat)) room.AddUser(su, Seat);
        else su.Error();
      }
      else
      {
        var room = server.GetRoom(fr.Id);
        if (Room == 0) room.RemoveUser(su);
        else if (fr.IsValidSeat(Seat)) room.ChangeSeat(su, Seat);
        else su.Error();
      }
    }
  }
}
