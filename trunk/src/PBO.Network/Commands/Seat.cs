using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network.Commands
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class SetSeat : IC2S
  {
    [DataMember(Name = "a", EmitDefaultValue = false)]
    public readonly int RoomId;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    public readonly Seat Seat;

    public SetSeat(int room, Seat seat)
    {
      RoomId = room;
      Seat = seat;
    }
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  public class SetUserSeat : S2C
  {
    public static SetUserSeat NewRoom(int user, int room, GameInitSettings settings)
    {
      return new SetUserSeat(user, room, Seat.Player00, settings);
    }
    public static SetUserSeat ChangeSeat(int user, int room, Seat seat)
    {
      return new SetUserSeat(user, room, seat, null);
    }
    public static SetUserSeat LeaveRoom(int user)
    {
      return new SetUserSeat(user, 0, 0, null);
    }

    
    [DataMember(Name = "a")]
    private readonly int User;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    private readonly int Room;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    private readonly Seat Seat;
    [DataMember(Name = "d", EmitDefaultValue = false)]
    private readonly GameInitSettings GameSettings;

    private SetUserSeat(int u, int r, Seat s, GameInitSettings settings)
    {
      User = u;
      Room = r;
      Seat = s;
      GameSettings = settings;
    }

    public override void Execute(Client client)
    {
      var user = client.State.GetUser(User);
      if (user != null)
      {
        if (Room == 0)
        {
          var room = user.Room;
          var seat = user.Seat;
          if (seat == Seat.Spectator) room.Spectators.Remove(user);
          else room[seat] = null;
          room.Battling = false;
          user.Room = null;
        }
        else if (GameSettings == null)
        {
          var room = client.State.GetRoom(Room);
          user.Seat = Seat;
          if (user.Room != room)
          {
            user.Room = room;
            if (Seat == Seat.Spectator) room.Spectators.Add(user);
            else room[Seat] = user;
          }
        }
        else
        {
          var room = new Room(Room, GameSettings, user);
          user.Room = room;
          user.Seat = Seat.Player00;
        }
      }
    }
  }
}
