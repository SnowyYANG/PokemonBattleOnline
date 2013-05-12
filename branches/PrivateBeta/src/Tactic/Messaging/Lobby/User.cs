using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace LightStudio.Tactic.Messaging
{
  public interface IBytable
  {
    void WriteToByte(BinaryWriter writer);
    void SetValue(BinaryReader reader);
  }
  
  public sealed class User<T> where T : IBytable, new()
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
      Extension = new T();
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
    public UserState State { get; internal set; }
    public string Sign { get; internal set; }
    public T Extension { get; private set; }
  }
}
