using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Messaging
{
  internal static class IOExtensions
  {
    public static GameSettings ReadSettings(this BinaryReader reader)
    {
      GameSettings s = new GameSettings((GameMode)reader.ReadByte());
      s.PPUp = reader.ReadDouble();
      int n = reader.ReadInt32();
      while (n-- > 0)
        s.AddRule(GameService.GetRule(reader.ReadInt32()));
      return s;
    }

    public static void WriteSettings(this BinaryWriter writer, GameSettings settings)
    {
      writer.Write((byte)settings.Mode);
      writer.Write(settings.PPUp);
      writer.Write(settings.ChosenRules.Count());
      foreach (Rule r in settings.ChosenRules)
        writer.Write(r.Id);
    }
  }
}
