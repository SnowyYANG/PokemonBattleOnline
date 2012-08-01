using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LightStudio.Tactic.Messaging;

namespace LightStudio.PokemonBattle.Messaging
{
  public class UserExtension : IBytable
  {
    public short LastRoomId
    { get; internal set; }

    public void WriteToByte(BinaryWriter writer)
    {
      writer.Write(LastRoomId);
    }
    public void SetValue(BinaryReader reader)
    {
      LastRoomId = reader.ReadInt16();
    }
  }
}
