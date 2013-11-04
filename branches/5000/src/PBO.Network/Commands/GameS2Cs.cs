using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network.Commands
{
  [DataContract(Name = "sp", Namespace = PBOMarks.JSON)]
  public class SetPrepare : IS2C
  {
    [DataMember(Name = "a", EmitDefaultValue = false)]
    Seat Seat;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    bool Prepare;

    public SetPrepare(Seat seat, bool prepare)
    {
      Seat = seat;
      Prepare = prepare;
    }

    void IS2C.Execute(Client client)
    {
      var room = client.Controller.Room;
      switch (Seat)
      {
        case Seat.Player00:
          room.Prepare00 = Prepare;
          break;
        case Seat.Player01:
          room.Prepare01 = Prepare;
          break;
        case Seat.Player10:
          room.Prepare10 = Prepare;
          break;
        case Seat.Player11:
          room.Prepare11 = Prepare;
          break;
      }
    }
  }
  
  [DataContract(Name = "pi", Namespace = PBOMarks.JSON)]
  public class PartnerInfoS2C : IS2C
  {
    [DataMember]
    PokemonData[] a_;

    public PartnerInfoS2C(IPokemonData[] pms)
    {
      a_ = pms.Select((p) => (PokemonData)p).ToArray();
    }

    void IS2C.Execute(Client client)
    {
      client.Controller.Room.Partner = a_;
    }
  }

  [DataContract(Name = "ri", Namespace = PBOMarks.JSON)]
  public class RequireInputS2C : InputRequest, IS2C
  {
    public RequireInputS2C(InputRequest ir)
      : base(ir)
    {
    }

    void IS2C.Execute(Client client)
    {
      client.Controller.Room.InputRequest = this;
    }
  }

  [DataContract(Name = "wi", Namespace = PBOMarks.JSON)]
  public class WaitingForInputS2C : IS2C
  {
    [DataMember(Name = "a")]
    int[] Players;

    public WaitingForInputS2C(int[] players)
    {
      Players = players;
    }

    void IS2C.Execute(Client client)
    {
      RoomController.OnTimeReminder(Players.Select((p) => client.Controller.GetUser(p)).ToArray());
    }
  }

  [DataContract(Name = "gu", Namespace = PBOMarks.JSON)]
  public class GameUpdateS2C : ReportFragment, IS2C
  {
    public GameUpdateS2C(ReportFragment rf)
      : base(rf)
    {
    }

    void IS2C.Execute(Client client)
    {
      var room = client.Controller.Room;
      if (room.Game == null) room.GameStart(this);
      room.Game.Update(Events);
      if (room.PlayerController != null)
      {
        room.PlayerController.Game.Update(this);
        if (room.InputRequest != null)
        {
          room.PlayerController.OnRequireInput(room.InputRequest);
          room.InputRequest = null;
        }
      }
    }
  }

  [DataContract(Name = "ge", Namespace = PBOMarks.JSON)]
  public class GameEndS2C : IS2C
  {
    public static GameEndS2C GameStop(int player, GameStopReason reason)
    {
      return new GameEndS2C() { Player = player, Reason = reason };
    }
    public static GameEndS2C TimeUp(IEnumerable<KeyValuePair<int, int>> time)
    {
      return new GameEndS2C() { Time = time.ToArray() };
    }
    [DataMember(EmitDefaultValue = false)]
    int Player;
    [DataMember(EmitDefaultValue = false)]
    GameStopReason Reason;

    [DataMember(EmitDefaultValue = false)]
    KeyValuePair<int, int>[] Time;

    private GameEndS2C()
    {
    }
    void IS2C.Execute(Client client)
    {
      if (Player != 0) RoomController.OnGameStop(Reason, client.Controller.GetUser(Player));
      else if (Time != null) RoomController.OnTimeUp(Time.Select((p) => new KeyValuePair<User, int>(client.Controller.GetUser(p.Key), p.Value)).ToArray());
      client.Controller.Room.Reset();
    }
  }
}
