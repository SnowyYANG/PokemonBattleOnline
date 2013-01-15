using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Messaging.Room
{
  public enum GameStopReason
  {
    PlayerGiveUp,
    PlayerDisconnect,
    InvalidInput,
    RoomClosed,
    RoomDisconnect,
    ServerClosed
  }
  internal interface IGameInformer
  {
    void InformGameTie();
    void InformGameStop(GameStopReason reason, int player);
    void InformTimeUp(IEnumerable<KeyValuePair<int, int>> remainingTime);
    void InformWaitingForInput(int[] players);

    void InformPlayerInfo(int teamIndex, Data.IPokemonData[] parnerPokemons);
    void InformReportUpdate(ReportFragment fragment);
    void InformRequireInput(InputRequest pms, int time);
    
    void InformRequestTie();
    void InformTieRejected();
  }

  [DataContract(Namespace = Namespaces.JSON)]
  class PlayerInfo : UserInformation
  {
    [DataMember(EmitDefaultValue = false)]
    int TeamIndex;

    [DataMember(EmitDefaultValue = false)]
    Data.IPokemonData[] Parner;

    public PlayerInfo(int teamIndex, Data.IPokemonData[] parner)
    {
      TeamIndex = teamIndex;
      Parner = parner;
    }

    public override void Execute(IRoomUser user)
    {
      user.InformPlayerInfo(TeamIndex, Parner);
    }
  }

  [DataContract(Namespace = Namespaces.JSON)]
  class GameEndInfo : UserInformation
  {
    public static GameEndInfo GameTie()
    {
      return new GameEndInfo();
    }
    public static GameEndInfo GameStop(int player, GameStopReason reason)
    {
      return new GameEndInfo() { Player = player, Reason = reason };
    }
    public static GameEndInfo TimeUp(IEnumerable<KeyValuePair<int, int>> time)
    {
      return new GameEndInfo() { Time = time.ToArray() };
    }
    [DataMember(EmitDefaultValue = false)]
    int Player;
    [DataMember(EmitDefaultValue = false)]
    GameStopReason Reason;

    [DataMember(EmitDefaultValue = false)]
    KeyValuePair<int, int>[] Time;

    private GameEndInfo()
    {
    }
    public override void Execute(IRoomUser user)
    {
      if (Player != 0) user.InformGameStop(Reason, Player);
      else if (Time != null) user.InformTimeUp(Time);
      else user.InformGameTie();
    }
  }

  [DataContract(Name = "w", Namespace = Namespaces.JSON)]
  class WaitingForInputInfo : UserInformation
  {
    [DataMember(Name = "a")]
    int[] Players;

    public WaitingForInputInfo(IEnumerable<int> players)
    {
      Players = players.ToArray();
    }

    public override void Execute(IRoomUser user)
    {
      user.InformWaitingForInput(Players);
    }
  }

  [DataContract(Name = "l", Namespace = Namespaces.JSON)]
  class ReportUpdateInfo : UserInformation
  {
    [DataMember(Name = "a")]
    ReportFragment Fragment;

    public ReportUpdateInfo(ReportFragment turn)
    {
      Fragment = turn;
    }
    public override void Execute(IRoomUser user)
    {
      user.InformReportUpdate(Fragment);
    }
  }

  [DataContract(Name = "r", Namespace = Namespaces.JSON)]
  class RequireInputInfo : UserInformation
  {
    [DataMember(Name = "a")]
    InputRequest PmInfo;

    [DataMember(Name = "b", EmitDefaultValue = false)]
    int SpentTime;

    public RequireInputInfo(InputRequest pmInfo, int spentTime)
    {
      PmInfo = pmInfo;
      SpentTime = spentTime;
    }
    public override void Execute(IRoomUser user)
    {
      user.InformRequireInput(PmInfo, SpentTime);
    }
  }
  
  [DataContract(Namespace = Namespaces.JSON)]
  class RequestTieInfo : UserInformation
  {
    public override void Execute(IRoomUser user)
    {
      user.InformRequestTie();
    }
  }

  [DataContract(Namespace = Namespaces.JSON)]
  class RejectTieInfo : UserInformation
  {
    public override void Execute(IRoomUser user)
    {
      user.InformTieRejected();
    }
  }
}