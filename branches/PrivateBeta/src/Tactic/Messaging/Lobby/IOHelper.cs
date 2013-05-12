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
    public static Avatar ReadAvatar(this BinaryReader reader)
    {
      byte id = reader.ReadByte();
      string url = reader.ReadString();
      return new Avatar(id, url);
    }
    public static void WriteAvatar(this BinaryWriter writer, Avatar avatar)
    {
      writer.Write(avatar.InnerAvatarId);
      writer.Write(avatar.Url);
    }
    public static User<T> ReadUser<T>(this BinaryReader reader) where T : IBytable, new()
    {
      int id = reader.ReadUserId();
      string name = reader.ReadString();
      Avatar avatar = reader.ReadAvatar();
      User<T> u = new User<T>(id, name, avatar);
      u.State = reader.ReadUserState();
      u.Sign = reader.ReadString();
      u.Extension.SetValue(reader);
      return u;
    }
    public static void WriteUser<T>(this BinaryWriter writer, User<T> user) where T : IBytable, new()
    {
      writer.WriteUserId(user.Id);
      writer.Write(user.Name);
      writer.WriteAvatar(user.Avatar);
      writer.WriteUserState(user.State);
      writer.Write(user.Sign);
      user.Extension.WriteToByte(writer);
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
