using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
  public enum PokemonNature : byte
  {
    Hardy,
    Lonely,
    Brave,
    Adamant,
    Naughty,
    Bold,
    Docile,
    Relaxed,
    Impish,
    Lax,
    Timid,
    Hasty,
    Serious,
    Jolly,
    Naive,
    Modest,
    Mild,
    Quiet,
    Bashful,
    Rash,
    Calm,
    Gentle,
    Sassy,
    Careful,
    Quirky,
  }
  public static class PokemonNatureHelper
  {
    private static readonly sbyte[,] REVISES = new sbyte[,]
    {
      { 10, 10, 10, 10, 10 },
      { 11, 9, 10, 10, 10 },
      { 11, 10, 10, 10, 9 },
      { 11, 10, 9, 10, 10 },
      { 11, 10, 10, 9, 10 },
      { 9, 11, 10, 10, 10 },
      { 10, 10, 10, 10, 10 },
      { 10, 11, 10, 10, 9 },
      { 10, 11, 9, 10, 10 },
      { 10, 11, 10, 9, 10 },
      { 9, 10, 10, 10, 11 },
      { 10, 9, 10, 10, 11 },
      { 10, 10, 10, 10, 10 },
      { 10, 10, 9, 10, 11 },
      { 10, 10, 10, 9, 11 },
      { 9, 10, 11, 10, 10 },
      { 10, 9, 11, 10, 10 },
      { 10, 10, 11, 10, 9 },
      { 10, 10, 10, 10, 10 },
      { 10, 10, 11, 9, 10 },
      { 9, 10, 10, 11, 10 },
      { 10, 9, 10, 11, 10 },
      { 10, 10, 10, 11, 9 },
      { 10, 10, 9, 11, 10 },
      { 10, 10, 10, 10, 10 }
    };

    public static int StatRevise(this PokemonNature nature, StatType stat)
    {
      return REVISES[(int)nature, (int)stat - 1];
    }
    public static PokemonNature? GetNature(string name)
    {
      var es = (PokemonNature[])Enum.GetValues(typeof(PokemonNature));
      foreach (var e in es)
        if (e.ToString() == name || e.GetLocalizedName() == name) return e;
      return null;
    }
    public static bool DislikeTaste(this PokemonNature nature, StatType stat)
    {
      return StatRevise(nature, stat) == 9;
    }
  }
}
