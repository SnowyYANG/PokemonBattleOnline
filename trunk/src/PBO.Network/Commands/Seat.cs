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
    public static SetSeatC2S NewRoom(GameSettings settings, Seat seat)
    {
      return new SetSeatC2S(0, seat, settings);
    }
    public static SetSeatC2S EnterRoom(int room, Seat seat)
    {
      return new SetSeatC2S(room, seat, null);
    }
    public static SetSeatC2S ChangeSeat(int room, Seat seat)
    {
      return new SetSeatC2S(-1, seat, null);
    }
    public static SetSeatC2S LeaveRoom()
    {
      return new SetSeatC2S(0, 0, null);
    }

    [DataMember(Name = "a", EmitDefaultValue = false)]
    public readonly int Room;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    public readonly Seat Seat;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    public readonly GameSettings GameSettings;

    private SetSeatC2S(int room, Seat seat, GameSettings settings)
    {
      Room = room;
      Seat = seat;
      GameSettings = settings;
    }
    protected SetSeatC2S()
    {
    }
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  public class SetSeatS2C : IS2C
  {
    public static SetSeatS2C EnterRoom(int user, int room, Seat seat)
    {
      return new SetSeatS2C(user, room, seat);
    }
    public static SetSeatS2C ChangeSeat(User user, Seat seat)
    {
      return new SetSeatS2C(user.Id, user.Room.Id, seat);
    }
    public static SetSeatS2C LeaveRoom(int user)
    {
      return new SetSeatS2C(user, 0, 0);
    }

    [DataMember(Name = "a", EmitDefaultValue = false)]
    private readonly int User;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    private readonly int Room;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    private readonly Seat Seat;

    private SetSeatS2C(int u, int r, Seat s)
    {
      User = u;
      Room = r;
      Seat = s;
    }

    void IS2C.Execute(Client client)
    {
      var user = client.State.GetUser(User);
      if (user != null)
        if (Room == 0)
        {
          var room = user.Room;
          var seat = user.Seat;
          if (seat == Seat.Spectator) room.RemoveSpectator(user);
          else room[seat] = null;
        }
        else
        {
          var room = client.State.GetRoom(Room);
          if (Seat == Seat.Spectator) room.AddSpectator(user);
          else room[Seat] = user;
        }
    }
  }
}
