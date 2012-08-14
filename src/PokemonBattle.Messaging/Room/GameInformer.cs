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
    void InformGameResult(int team0, int team1);
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
    public static GameEndInfo TimeUp(IEnumerable<KeyValuePair<int, int>> time)
    {
      return new GameEndInfo() { Time = time.ToArray() };
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
    KeyValuePair<int, int>[] Time;

    private GameEndInfo()
    {
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      if (RemaningPokemons != null) user.InformGameResult(RemaningPokemons[0], RemaningPokemons[1]);
      else if (IsTile) user.InformGameTie();
      else if (Time != null) user.InformTimeUp(Time);
      else user.InformGameStop(Reason, Player);
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