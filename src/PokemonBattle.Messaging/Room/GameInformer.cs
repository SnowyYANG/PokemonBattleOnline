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
    UserQuit,
    UserDisconnect,
    InvalidInput,
    RoomClosed,
    RoomDisconnect,
    ServerClosed
  }
  internal interface IGameInformer
  {
    void InformGameResult(int team0, int team1);
    void InformGameTie();
    void InformGameStop(GameStopReason reason, int player);
    void InformTimeUp(int[] remainingTime);

    void InformReportUpdate(ReportFragment fragment);
    void InformRequireInput(InputRequest pms);
    
    void InformRequestTie();
    void InformTieRejected();
  }

  [DataContract(Namespace = Namespaces.LIGHT)]
  class GameEndInfo : IUserInformation
  {
    public static GameEndInfo GameResult(int team0, int team1)
    {
      GameEndInfo info = new GameEndInfo() { RemaningPokemons = new int[2] };
      info.RemaningPokemons[0] = team0;
      info.RemaningPokemons[1] = team1;
      return info;
    }
    public static GameEndInfo GameTie()
    {
      return new GameEndInfo() { IsTile = true };
    }
    public static GameEndInfo GameStop(int player, GameStopReason reason)
    {
      return new GameEndInfo() { Player = player, Reason = reason };
    }
    public static GameEndInfo TimeUp(int[] remainingTime)
    {
      return new GameEndInfo() { RemainingTime = remainingTime };
    }
    /// <summary>
    /// null if user tie
    /// </summary>
    [DataMember(EmitDefaultValue = false)]
    int[] RemaningPokemons;

    [DataMember(EmitDefaultValue = false)]
    bool IsTile;

    [DataMember(EmitDefaultValue = false)]
    int Player;
    [DataMember(EmitDefaultValue = false)]
    GameStopReason Reason;

    [DataMember(EmitDefaultValue = false)]
    int[] RemainingTime;

    private GameEndInfo()
    {
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      if (RemaningPokemons != null) user.InformGameResult(RemaningPokemons[0], RemaningPokemons[1]);
      else if (IsTile) user.InformGameTie();
      else if (RemainingTime != null) user.InformTimeUp(RemainingTime);
      else user.InformGameStop(Reason, Player);
    }
  }

  [DataContract(Namespace = Namespaces.LIGHT)]
  class ReportUpdateInfo : IUserInformation
  {
    [DataMember]
    ReportFragment Fragment;

    [DataMember(EmitDefaultValue = false)]
    Player[] Leap;

    /// <summary>
    /// 非回合开始时的所有玩家选招，如飞天、逆鳞，注意：Leap（观战/首回合）的战报段亦可能SP（替代物）
    /// </summary>
    [DataMember(EmitDefaultValue = false)]
    bool HasAddition;

    /// <summary>
    /// 如果要wifi模式计时器的话就null吧，以及计时器放在Host这吧
    /// </summary>
    [DataMember(EmitDefaultValue = false)]
    int?[] Seconds;

    public ReportUpdateInfo(ReportFragment turn, bool hasAddition)
    {
      Fragment = turn;
      HasAddition = hasAddition;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformReportUpdate(Fragment);
      if (!HasAddition) user.InformRequireInput(null);
    }
  }

  [DataContract(Namespace = Namespaces.LIGHT)]
  class RequireInputInfo : IUserInformation
  {
    [DataMember]
    InputRequest PmInfo;

    public RequireInputInfo(InputRequest pmInfo)
    {
      PmInfo = pmInfo;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformRequireInput(PmInfo);
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