using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network.Commands
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class SetSeatC2S : IC2S
  {
    [DataMember(Name = "a", EmitDefaultValue = false)]
    public readonly int RoomId;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    public readonly Seat Seat;

    public SetSeatC2S(int room, Seat seat)
    {
      RoomId = room;
      Seat = seat;
    }
    protected SetSeatC2S()
    {
    }
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  public class SetSeatS2C : S2C
  {
    public static SetSeatS2C NewRoom(int user, int room, GameInitSettings settings)
    {
      return new SetSeatS2C(user, room, Seat.Player00, settings);
    }
    public static SetSeatS2C ChangeSeat(int user, int room, Seat seat)
    {
      return new SetSeatS2C(user, room, seat, null);
    }
    public static SetSeatS2C LeaveRoom(int user)
    {
      return new SetSeatS2C(user, 0, 0, null);
    }

    [DataMember(Name = "a")]
    private readonly int User;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    private readonly int Room;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    private readonly Seat Seat;
    [DataMember(Name = "d", EmitDefaultValue = false)]
    private readonly GameInitSettings GameSettings;

    private SetSeatS2C(int u, int r, Seat s, GameInitSettings settings)
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
          var room = new Room(Room, GameSettings);
          room[Seat] = user;
          user.Room = room;
          user.Seat = Seat;
        }
      }
    }
  }
}
