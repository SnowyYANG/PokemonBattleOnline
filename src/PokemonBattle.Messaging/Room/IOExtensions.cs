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
      int n = reader.ReadInt32();
      while (n-- > 0)
        s.AddRule(GameService.GetRule(reader.ReadInt32()));
      return s;
    }

    public static void WriteSettings(this BinaryWriter writer, GameInitSettings settings)
    {
      writer.Write((byte)settings.Mode);
      writer.Write(settings.Rules.Count());
      foreach (Rule r in settings.Rules)
        writer.Write(r.Id);
    }
  }
}
