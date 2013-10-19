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
      throw new NotImplementedException();
    }
  }
  
  [DataContract(Name = "pi", Namespace = PBOMarks.JSON)]
  public class ParnerInfoS2C : IS2C
  {
    [DataMember]
    PokemonData[] a_;

    public ParnerInfoS2C(IPokemonData[] pms)
    {
      a_ = pms.Select((p) => (PokemonData)p).ToArray();
    }

    void IS2C.Execute(Client client)
    {
      throw new NotImplementedException();
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
      throw new NotImplementedException();
    }
  }

  [DataContract(Name = "wi", Namespace = PBOMarks.JSON)]
  public class WaitingForInputS2C : IS2C
  {
    [DataMember(Name = "a")]
    int[] Players;

    public WaitingForInputS2C(IEnumerable<int> players)
    {
      Players = players.ToArray();
    }

    void IS2C.Execute(Client client)
    {
      throw new NotImplementedException();
      //user.InformWaitingForInput(Players);
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
      throw new NotImplementedException();
    }
  }

  [DataContract(Name = "ge", Namespace = PBOMarks.JSON)]
  public class GameEndS2C : IS2C
  {
    //public static GameEndInfo GameTie()
    //{
    //  return new GameEndInfo();
    //}
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
      throw new NotImplementedException();
      //      if (Player != 0) user.InformGameStop(Reason, Player);
      //      else if (Time != null) user.InformTimeUp(Time);
    }
  }
}
