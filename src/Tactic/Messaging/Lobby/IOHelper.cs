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
    public static User<T> ReadUser<T>(this BinaryReader reader) where T : new()
    {
      int id = reader.ReadUserId();
      string name = reader.ReadString();
      Avatar avatar = reader.ReadAvatar();
      User<T> u = new User<T>(id, name, avatar);
      u.State = reader.ReadUserState();
      u.Sign = reader.ReadString();
      return u;
    }
    public static void WriteUser<T>(this BinaryWriter writer, IUser<T> user)
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

  /// <summary>
  /// i hate delegates' performance
  /// Also I think these should be IMessage.Read(... method
  /// static TextMessage.Create(... & TextMessage should have no public constructor
  /// </summary>
  internal static class MessageHelper
  {
    public static IMessage BuildMessage(string header, Action<BinaryWriter> buildcontent = null)
    {
      TextMessage m = new TextMessage();
      m.Header = header;
      if (buildcontent != null)
        using (var stream = new MemoryStream())
        {
          var writer = new BinaryWriter(stream);
          buildcontent(writer);
          m.Content = Convert.ToBase64String(stream.ToArray());
        }
      return m;
    }
    public static void ResolveMessage(IMessage message, Action<BinaryReader> resolvecontent)
    {
      using (var stream = new MemoryStream(Convert.FromBase64String(message.Content)))
      {
        var reader = new BinaryReader(stream);
        resolvecontent(reader);
      }
    }
  }
}
