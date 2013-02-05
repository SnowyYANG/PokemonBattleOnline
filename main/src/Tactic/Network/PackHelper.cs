using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Tactic.Network
{
  public static class PackHelper
  {
    private static readonly byte[] EMPTYPACK;

    static PackHelper()
    {
      EMPTYPACK = new byte[0];
    }

    public static void SendEmpty(this INetworkIO network)
    {
      network.Send(EMPTYPACK);
    }
    public static bool IsEmpty(this byte[] pack)
    {
      return pack.Length == 0;
    }

    public static void Send(this INetworkIO network, byte pack)
    {
      network.Send(new byte[] { pack });
    }
    public static byte? ToByte(this byte[] pack)
    {
      return pack.Length == 1 ? (byte?)pack[0] : null;
    }

    public static void Send(this INetworkIO network, UInt16 pack)
    {
      network.Send(ToPack(pack));
    }
    public static byte[] ToPack(this UInt16 pack)
    {
      return new byte[] { (byte)(pack >> 8), (byte)pack };
    }
    public static UInt16? ToInt16(this byte[] pack)
    {
      return pack.Length == 2 ? (UInt16?)((pack[0] << 8) | pack[1]) : null;
    }

    public static void Send(this INetworkIO network, string pack)
    {
      network.Send(Encoding.Unicode.GetBytes(pack));
    }
    public static string ToUnicodeString(this byte[] pack)
    {
      return Encoding.Unicode.GetString(pack);
    }

    public static void Send<T>(this INetworkIO network, T obj)
    {
      network.Send(Serializer.SerializeToJson(obj));
    }
    public static T ToObject<T>(this byte[] pack)
    {
      return Serializer.DeserializeFromJson<T>(pack);
    }

    public static void Send(this INetworkIO network, byte header, byte[] content)
    {
      var pack = new byte[content.Length + 1];
      pack[0] = header;
      var packContent = new ArraySegment<byte>(pack, 1, content.Length);
      Array.Copy(content, 0, pack, 1, content.Length);
      network.Send(pack);
    }
    public static byte GetHeader(this byte[] pack)
    {
      return pack[0];
    }
    public static byte[] GetContent(this byte[] pack)
    {
      return new ArraySegment<byte>(pack, 1, pack.Length - 1).Array;
    }
  }
}
