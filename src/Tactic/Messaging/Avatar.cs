using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Messaging
{
  /// <summary>
  /// You can only get an avatar instance from UserInfo or BinaryStream
  /// </summary>
  public class Avatar
  {
    public static string FormatUrl(string url)
    {
      if (Uri.IsWellFormedUriString(url, UriKind.Absolute)) return url;
      else return string.Empty;
    }

    byte innerAvatarId;
    string url;

    internal Avatar(byte id, string url)
    {
      innerAvatarId = id;
      this.url = FormatUrl(url);
    }
    internal Avatar(byte id) : this(id, null)
    {
    }

    public byte InnerAvatarId
    { get { return innerAvatarId; } }
    public string Url
    { get { return url; } }
  }
}
