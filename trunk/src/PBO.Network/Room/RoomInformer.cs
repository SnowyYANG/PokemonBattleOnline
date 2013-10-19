﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections.ObjectModel;
//using System.Runtime.Serialization;

//namespace PokemonBattleOnline.Network.Commands
//{
//  [DataContract(Name = "ji", Namespace = PBOMarks.JSON)]
//  class UserJoinGameInfo : UserInformation
//  {
//    [DataMember(Name = "a", EmitDefaultValue = false)]
//    public int UserId
//    { get; private set; }

//    [DataMember(Name = "b", EmitDefaultValue = false)]
//    public int TeamId
//    { get; private set; }

//    public UserJoinGameInfo(int userId, int teamId)
//    {
//      this.UserId = userId;
//      this.TeamId = teamId;
//    }
//    public override void Execute(IRoomUser user)
//    {
//      user.InformUserJoinGame(UserId, TeamId);
//    }
//  }

//  [DataContract(Namespace = PBOMarks.JSON)]
//  class UserQuitInfo : UserInformation
//  {
//    [DataMember(EmitDefaultValue = false)]
//    public int UserId
//    { get; private set; }

//    public UserQuitInfo(int userId)
//    {
//      this.UserId = userId;
//    }
//    public override void Execute(IRoomUser user)
//    {
//      user.InformUserQuit(UserId);
//    }
//  }

//  [DataContract(Name = "js", Namespace = PBOMarks.JSON)]
//  class EnterSucceedInfo : UserInformation
//  {
//    public static EnterSucceedInfo Player()
//    {
//      throw new NotImplementedException();
//      //return new EnterSucceedInfo(host.GameSettings, host.Players, host.Spectators);
//    }
//    /// <param name="leapTurn">it can be null</param>
//    public static EnterSucceedInfo Spectator()
//    {
//      throw new NotImplementedException();
//      //return new EnterSucceedInfo(host.GameSettings, host.Players, host.Spectators) { Leap = leapTurn };
//    }
    
//    [DataMember(Name = "a")]
//    public GameInitSettings Settings
//    { get; private set; }

//    [DataMember(Name = "b")]
//    Player[] Players;

//    [DataMember(Name = "c")]
//    int[] Spectators;

//    [DataMember(Name = "d", EmitDefaultValue = false)]
//    Game.ReportFragment Leap; //spectator only

//    private EnterSucceedInfo(GameInitSettings settings, IEnumerable<Player> players, IEnumerable<int> spectators)
//    {
//      this.Settings = settings;
//      Players = players.ToArray();
//      Spectators = spectators.ToArray();
//    }
//    public override void Execute(IRoomUser user)
//    {
//      user.InformEnterSucceed(Settings, Players, Spectators);
//      if (Leap != null) user.InformReportUpdate(Leap);
//    }
//  }
//}