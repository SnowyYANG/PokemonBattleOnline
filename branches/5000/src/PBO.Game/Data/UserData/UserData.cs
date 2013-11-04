using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace PokemonBattleOnline.Game
{
  public static class UserData
  {
    private static string FileName;

    public static void Load(string fileName)
    {
      FileName = fileName;
      try
      {
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
          Current = Serializer.Deserialize<PokemonTeam[]>(fs);
      }
      catch
      {
        Current = Enumerable.Empty<PokemonTeam>();
      }
    }

    public static IEnumerable<PokemonTeam> Current
    { get; private set; }

    public static PokemonTeam ImportTeam(string source)
    {
      var pms = new PokemonData[6];
      Helper.Import(source, pms);
      return new PokemonTeam(pms);
    }
    public static PokemonData ImportPokemon(string source)
    {
      var target = new PokemonData[1];
      Helper.Import(source, target);
      return target[0];
    }
    public static string Export(PokemonData pm)
    {
      var sb = new StringBuilder();
      Helper.Export(sb, pm);
      return sb.ToString();
    }
    public static string Export(IEnumerable<PokemonData> pms)
    {
      var sb = new StringBuilder();
      bool first = true;
      foreach (var pm in pms)
      {
        if (first) first = false;
        else
        {
          sb.AppendLine();
          sb.AppendLine();
        }
        Helper.Export(sb, pm);
      }
      return sb.ToString();
    }

    public static void Save(IEnumerable<PokemonTeam> teams)
    {
      Current = teams.ToArray();
      using (var f = new FileStream(FileName, FileMode.Create, FileAccess.Write))
        Serializer.Serialize(Current, f);
    }
  }
}
