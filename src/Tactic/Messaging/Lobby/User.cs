using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace LightStudio.Tactic.Messaging.Lobby
{
  public enum UserState : byte
  {
    /// <summary>
    /// login not complete, avatar isn't uploaded
    /// </summary>
    Invalid = 0,
    Normal,
    Aggressive,
    Battling,
    Watching,
    Afk
  }
  
  public sealed class User
  {
    int id;
    string name;
    Avatar avatar;

    public User(int id, string name, Avatar avatar)
    {
      this.id = id;
      this.name = name;
      this.avatar = avatar;
      Sign = string.Empty;
    }
    public User(int id, string name) : this(id, name, null)
    {
    }
    public User(int id, string name, byte innerAvatarId, string avatarUrl)
      : this(id, name, new Avatar(innerAvatarId, avatarUrl))
    {
    }
    public User(int id, string name, byte avatar) : this(id, name, avatar, null)
    {
    }

    public int Id
    { get { return id; } }
    public string Name
    { get { return name; } }
    public Avatar Avatar
    {
      get { return avatar; }
      internal set { avatar = value; }
    }
    public UserState State { get; set; }
    public string Sign { get; set; }
  }
}
