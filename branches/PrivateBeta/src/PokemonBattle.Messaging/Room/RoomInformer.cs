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

  [DataContract(Namespace = PBOMarks.PBO)]
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

  [DataContract(Name = "ji", Namespace = PBOMarks.PBO)]
  class UserJoinGameInfo : IUserInformation
  {

    [DataMember(Name = "a", EmitDefaultValue = false)]
    public int UserId
    { get; private set; }

    [DataMember(Name = "b", EmitDefaultValue = false)]
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

  [DataContract(Namespace = PBOMarks.PBO)]
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

  [DataContract(Namespace = PBOMarks.PBO)]
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

  [DataContract(Namespace = PBOMarks.PBO)]
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

  [DataContract(Name = "js", Namespace = PBOMarks.PBO)]
  class EnterSucceedInfo : IUserInformation
  {
    public static EnterSucceedInfo Player(Host host)
    {
      return new EnterSucceedInfo(host.GameSettings, host.Players, host.Spectators);
    }
    /// <param name="leapTurn">it can be null</param>
    public static EnterSucceedInfo Spectator(Host host, Game.ReportFragment leapTurn)
    {
      return new EnterSucceedInfo(host.GameSettings, host.Players, host.Spectators) { Leap = leapTurn };
    }
    
    [DataMember(Name = "a")]
    public GameInitSettings Settings
    { get; private set; }

    [DataMember(Name = "b")]
    Player[] Players;

    [DataMember(Name = "c")]
    int[] Spectators;

    [DataMember(Name = "d", EmitDefaultValue = false)]
    Game.ReportFragment Leap; //spectator only

    private EnterSucceedInfo(GameInitSettings settings, IEnumerable<Player> players, IEnumerable<int> spectators)
    {
      this.Settings = settings;
      Players = players.ToArray();
      Spectators = spectators.ToArray();
    }
    void IUserInformation.Execute(IRoomUser user)
    {
      user.InformEnterSucceed(Settings, Players, Spectators);
      if (Leap != null) user.InformReportUpdate(Leap);
    }
  }
}