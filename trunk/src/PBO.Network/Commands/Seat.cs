﻿using System;
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
    public static SetSeatC2S NewRoom(string name, GameSettings settings, Seat seat)
    {
      return new SetSeatC2S(0, seat, name, settings);
    }
    public static SetSeatC2S EnterRoom(int room, Seat seat)
    {
      return new SetSeatC2S(room, seat, null, null);
    }
    public static SetSeatC2S ChangeSeat(int room, Seat seat)
    {
      return new SetSeatC2S(-1, seat, null, null);
    }
    public static SetSeatC2S LeaveRoom()
    {
      return new SetSeatC2S(0, 0, null, null);
    }

    [DataMember(Name = "a", EmitDefaultValue = false)]
    public readonly int Room;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    public readonly Seat Seat;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    public readonly string Name;
    [DataMember(Name = "d", EmitDefaultValue = false)]
    public readonly GameSettings GameSettings;

    private SetSeatC2S(int room, Seat seat, string name, GameSettings settings)
    {
      Room = room;
      Seat = seat;
      Name = name;
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
      var u = client.Controller.GetUser(User);
      var isUser = u == client.Controller.User;
      if (u != null)
        if (Room == 0)
        {
          var room = u.Room;
          var seat = u.Seat;
          if (seat == Seat.Spectator) room.RemoveSpectator(u);
          else room[seat] = null;
          if (isUser) client.Controller.Room.Reset();
        }
        else
        {
          var room = client.Controller.GetRoom(Room);
          if (Seat == Seat.Spectator) room.AddSpectator(u);
          else room[Seat] = u;
        }
    }
  }
}
