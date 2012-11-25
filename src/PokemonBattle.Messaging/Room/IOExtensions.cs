using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Messaging.Room;

namespace LightStudio.PokemonBattle.Messaging
{
  internal static class IOExtensions
  {
    public static GameInitSettings ReadSettings(this BinaryReader reader)
    {
      GameInitSettings s = new GameInitSettings((GameMode)reader.ReadByte());
      return s;
    }

    public static void WriteSettings(this BinaryWriter writer, GameInitSettings settings)
    {
      writer.Write((byte)settings.Mode);
    }
  }
}
