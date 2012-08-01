using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  internal interface IRoomInformer
  {
    void InformUserSpectateGame(int userId);
    void InformUserJoinGame(int userId, int teamId);
    void InformUserQuit(int userId);
    void InformUserKicked(int userId);
    void InformEnterFailed(string message);//Join or Observe Game
    void InformEnterSucceed(GameInitSettings settings, Player[] players, int[] spectators);
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class UserSpectateGameInfo : IUserInformation
  {

    [DataMember(EmitDefaultValue = false)]
    public int UserId
    { get; private set; }

    public UserSpectateGameInfo(int userId)
    {
      this.UserId = userId;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformUserSpectateGame(UserId);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class UserJoinGameInfo : IUserInformation
  {

    [DataMember(EmitDefaultValue = false)]
    public int UserId
    { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public int TeamId
    { get; private set; }

    public UserJoinGameInfo(int userId, int teamId)
    {
      this.UserId = userId;
      this.TeamId = teamId;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformUserJoinGame(UserId, TeamId);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class UserQuitInfo : IUserInformation
  {

    [DataMember(EmitDefaultValue = false)]
    public int UserId
    { get; private set; }

    public UserQuitInfo(int userId)
    {

      this.UserId = userId;

    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformUserQuit(UserId);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class UserKickedInfo : IUserInformation
  {
    [DataMember(EmitDefaultValue = false)]
    public int UserId
    { get; private set; }

    public UserKickedInfo(int userId)
    {
      this.UserId = userId;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformUserKicked(UserId);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class EnterFailedInfo : IUserInformation
  {
    [DataMember(EmitDefaultValue = false)]
    public string Message
    { get; private set; }

    public EnterFailedInfo(string message)
    {
      this.Message = message;
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformEnterFailed(Message);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class EnterSucceedInfo : IUserInformation
  {
    public static EnterSucceedInfo Player(Host host, int[] ids)
    {
      return new EnterSucceedInfo(host.GameSettings, host.Players, host.Spectators) { Ids = ids };
    }
    /// <param name="leapTurn">it can be null</param>
    public static EnterSucceedInfo Spectator(Host host, Game.ReportFragment leapTurn)
    {
      return new EnterSucceedInfo(host.GameSettings, host.Players, host.Spectators) { Leap = leapTurn };
    }
    
    [DataMember]
    public GameInitSettings Settings
    { get; private set; }

    [DataMember]
    Player[] Players;

    [DataMember]
    int[] Spectators;

    [DataMember(EmitDefaultValue = false)]
    int[] Ids; //players only

    [DataMember(EmitDefaultValue = false)]
    Game.ReportFragment Leap; //spectator only

    private EnterSucceedInfo(GameInitSettings settings, IEnumerable<Player> players, IEnumerable<int> spectators)
    {
      this.Settings = settings;
      Players = players.ToArray();
      Spectators = spectators.ToArray();
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      if (Ids != null) Settings.SetIds(Ids); //序列化后
      user.InformEnterSucceed(Settings, Players, Spectators);
      if (Leap != null) user.InformReportUpdate(Leap);
    }
  }
}