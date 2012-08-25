using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  public enum GameStopReason
  {
    PlayerQuit,
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

    void InformReportUpdate(ReportFragment fragment);
    void InformRequireInput(InputRequest pms, int time);
    
    void InformRequestTie();
    void InformTieRejected();
  }

  [DataContract(Namespace = Namespaces.LIGHT)]
  class GameEndInfo : IUserInformation
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
    void IUserInformation.Execute(IRoomUser user)
    {
      if (Player != 0) user.InformGameStop(Reason, Player);
      else if (Time != null) user.InformTimeUp(Time);
      else user.InformGameTie();
    }
  }

  [DataContract(Namespace = Namespaces.LIGHT)]
  class WaitingForInputInfo : IUserInformation
  {
    [DataMember]
    int[] Players;

    public WaitingForInputInfo(IEnumerable<int> players)
    {
      Players = players.ToArray();
    }

    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformWaitingForInput(Players);
    }
  }

  [DataContract(Namespace = Namespaces.LIGHT)]
  class ReportUpdateInfo : IUserInformation
  {
    [DataMember]
    ReportFragment Fragment;

    public ReportUpdateInfo(ReportFragment turn)
    {
      Fragment = turn;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformReportUpdate(Fragment);
    }
  }

  [DataContract(Namespace = Namespaces.LIGHT)]
  class RequireInputInfo : IUserInformation
  {
    [DataMember]
    InputRequest PmInfo;

    [DataMember(EmitDefaultValue = false)]
    int SpentTime;

    public RequireInputInfo(InputRequest pmInfo, int spentTime)
    {
      PmInfo = pmInfo;
      SpentTime = spentTime;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformRequireInput(PmInfo, SpentTime);
    }
  }
  
  [DataContract(Namespace = Namespaces.LIGHT)]
  class RequestTieInfo : IUserInformation
  {
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformRequestTie();
    }
  }

  [DataContract(Namespace = Namespaces.LIGHT)]
  class RejectTieInfo : IUserInformation
  {
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformTieRejected();
    }
  }
}