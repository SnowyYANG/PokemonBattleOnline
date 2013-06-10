using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace LightStudio.Tactic.Messaging
{
  public sealed class User
  {
    int id;
    string name;
    int avatar;

    public User(int id, string name, int avatar)
    {
      this.id = id;
      this.name = name;
      this.avatar = avatar;
      Sign = string.Empty;
    }

    public int Id
    { get { return id; } }
    public string Name
    { get { return name; } }
    public int Avatar
    {
      get { return avatar; }
      internal set { avatar = value; }
    }
    public UserState State { get; internal set; }
    public string Sign { get; internal set; }
  }
}
