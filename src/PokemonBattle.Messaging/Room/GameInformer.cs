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
    void InformRequireInput(RequireInput pms);
    void InformPlayerInputed(int player);
    
    void InformRequestTie();
    void InformTieRejected();
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class GameEndInfo : IUserInformation
  {
    public static GameEndInfo GameResult(int team0, int team1)
    {
      GameEndInfo info = new GameEndInfo();
      info.RemaningPokemons[0] = team0;
      info.RemaningPokemons[1] = team1;
      return info;
    }
    public static GameEndInfo GameTie()
    {
      throw new NotImplementedException();
    }
    public static GameEndInfo GameStop(int player, GameStopReason reason)
    {
      throw new NotImplementedException();
    }
    public static GameEndInfo TimeUp(int[] remainingTime)
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// null if user tie
    /// </summary>
    [DataMember(EmitDefaultValue = false)]
    readonly int[] RemaningPokemons;

    private GameEndInfo()
    {
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      throw new NotImplementedException();
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class ReportUpdateInfo : IUserInformation
  {
    [DataMember]
    ReportFragment Fragment;

    [DataMember(EmitDefaultValue = false)]
    Player[] Leap;

    /// <summary>
    /// 非回合开始时的所有玩家选招，如飞天、逆鳞，注意：Leap（观战/首回合）的战报段没可能是SP的
    /// </summary>
    [DataMember(EmitDefaultValue = false)]
    bool HasAddition;

    /// <summary>
    /// 如果要wifi模式计时器的话就null吧，以及计时器放在Host这吧
    /// </summary>
    [DataMember(EmitDefaultValue = false)]
    int?[] Seconds;

    public ReportUpdateInfo(ReportFragment turn)
    {
      Fragment = turn;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      if (Fragment.Teams == null)
      {
        user.InformReportUpdate(Fragment);
        //seconds
        if (!HasAddition) user.InformRequireInput(null);
      }
      else
      {
        //leap
      }
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class RequireInputInfo : IUserInformation
  {
    [DataMember]
    RequireInput PmInfo;

    public RequireInputInfo(RequireInput pmInfo)
    {
      PmInfo = pmInfo;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformRequireInput(PmInfo);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class PlayerInputedInfo : IUserInformation
  {
    [DataMember]
    int Player;

    public PlayerInputedInfo(int userId)
    {
      Player = userId;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformPlayerInputed(Player);
    }
  }
  
  [DataContract(Namespace = Namespaces.DEFAULT)]
  class RequestTieInfo : IUserInformation
  {
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformRequestTie();
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class RejectTieInfo : IUserInformation
  {
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformTieRejected();
    }
  }
}