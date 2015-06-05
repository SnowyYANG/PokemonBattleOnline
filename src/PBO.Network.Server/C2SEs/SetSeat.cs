using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network.C2SEs
{
  [DataContract(Namespace = PBOMarks.JSON)]
  internal class SetSeatC2S : Commands.SetSeatC2S, IC2SE
  {
    public void Execute(ServerUser su)
    {
      var user = su.User;
      var server = su.Server;
      var fr = su.Room;
      if (fr == null)
      {
        var r = GameSettings == null ? server.GetRoom(Room) : server.AddRoom(Name, GameSettings);
        if (r != null && r.Room.IsValidSeat(Seat)) r.AddUser(su, Seat);
      }
      else
      {
        if (Room == 0) fr.RemoveUser(su);
        else if (fr.Room.IsValidSeat(Seat)) fr.ChangeSeat(su, Seat);
      }
    }
  }
}
