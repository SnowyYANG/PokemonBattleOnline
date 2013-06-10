using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LightStudio.Tactic.Messaging.Lobby;

namespace LightStudio.Tactic.Messaging
{
  internal static class IOExtensions
  {
    public static int ReadUserId(this BinaryReader reader)
    {
      return reader.ReadInt16();
    }
    public static void WriteUserId(this BinaryWriter writer, int id)
    {
      writer.Write((Int16)id);
    }
    public static UserState ReadUserState(this BinaryReader reader)
    {
      return (UserState)reader.ReadByte();
    }
    public static void WriteUserState(this BinaryWriter writer, UserState state)
    {
      writer.Write((byte)state);
    }
    public static int ReadAvatar(this BinaryReader reader)
    {
      return reader.ReadInt16();
    }
    public static void WriteAvatar(this BinaryWriter writer, int avatar)
    {
      writer.Write((short)avatar);
    }
    public static User ReadUser(this BinaryReader reader)
    {
      int id = reader.ReadUserId();
      string name = reader.ReadString();
      int avatar = reader.ReadAvatar();
      User u = new User(id, name, avatar);
      u.State = reader.ReadUserState();
      u.Sign = reader.ReadString();
      return u;
    }
    public static void WriteUser(this BinaryWriter writer, User user)
    {
      writer.WriteUserId(user.Id);
      writer.Write(user.Name);
      writer.WriteAvatar(user.Avatar);
      writer.WriteUserState(user.State);
      writer.Write(user.Sign);
    }

    public static TElement[] ReadArray<TElement>(this BinaryReader reader, Func<TElement> readElement)
    {
      int length = reader.ReadInt32();
      var array = new TElement[length];
      for (int i = 0; i < length; i++) array[i] = readElement();
      return array;
    }
    public static void WriteArray<TElement>(this BinaryWriter writer, IEnumerable<TElement> array, Action<TElement> writeElement)
    {
      writer.Write(array.Count());
      foreach (var item in array) writeElement(item);
    }
  }
}
