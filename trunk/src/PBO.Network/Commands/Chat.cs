using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network
{
  public enum ChatMode
  {
    Public,
    Room,
    Private
  }
  [DataContract(Namespace = PBOMarks.JSON)]
  public class ChatC2S : C2S
  {
    public static ChatC2S PublicChat(string info)
    {
      return new ChatC2S(ChatMode.Public, 0, info);
    }
    public static ChatC2S RoomChat(string info)
    {
      return new ChatC2S(ChatMode.Room, 0, info);
    }
    public static ChatC2S PrivateChat(int to, string info)
    {
      return new ChatC2S(ChatMode.Private, to, info);
    }

    [DataMember(Name = "b", EmitDefaultValue = false)]
    public readonly ChatMode Mode;

    /// <summary>
    /// private mode only
    /// </summary>
    [DataMember(Name = "c", EmitDefaultValue = false)]
    public readonly int To;

    [DataMember(Name = "a")]
    public readonly string Info;

    private ChatC2S(ChatMode mode, int to, string info)
    {
      Mode = mode;
      To = to;
      Info = info;
    }
  }
  internal class ChatS2C : S2C
  {
    public override void Execute(Client client)
    {
    }
  }
}
