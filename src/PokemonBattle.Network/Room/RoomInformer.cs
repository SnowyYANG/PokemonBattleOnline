using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Room
{
  internal interface IRoomInformer
  {
    void InformChat(int userId, MessageTarget target, string content);
    void InformUserSpectateGame(int userId);
    void InformUserJoinGame(int userId, int teamId);
    void InformUserQuit(int userId);
    void InformUserKicked(int userId);
    void InformEnterFailed(string message);//Join or Observe Game
    void InformEnterSucceed(Game.GameSettings settings, int[] players, int[] spectators, int[] ids);
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class ChatInfo : IUserInformation
  {

    [DataMember(EmitDefaultValue = false)]
    public int UserId
    { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public MessageTarget Target
    { get; set; }

    [DataMember]
    public string Content
    { get; set; }

    public ChatInfo(int userId, MessageTarget target, string content)
    {
      this.UserId = userId;
      this.Target = target;
      this.Content = content;
    }
    void IUserInformation.Execute(IUser user)
    {
      user.InformChat(UserId, Target, Content);
    }
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
    void IUserInformation.Execute(IUser user)
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
    void IUserInformation.Execute(IUser user)
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
    void IUserInformation.Execute(IUser user)
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
    void IUserInformation.Execute(IUser user)
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
    void IUserInformation.Execute(IUser user)
    {
      user.InformEnterFailed(Message);
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  class EnterSucceedInfo : IUserInformation
  {
    [DataMember(EmitDefaultValue = false)]
    public Game.GameSettings Settings
    { get; private set; }

    [DataMember]
    int[] Players;

    [DataMember]
    int[] Spectators;

    [DataMember(EmitDefaultValue = false)]
    int[] Ids;

    public EnterSucceedInfo(Game.GameSettings settings, int[] players, int[] spectators, int[] ids = null)
    {
      this.Settings = settings;
      Players = players;
      Spectators = spectators;
      Ids = ids;
    }
    void IUserInformation.Execute(IUser user)
    {
      user.InformEnterSucceed(Settings, Players, Spectators, Ids);
    }
  }
}