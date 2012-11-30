using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LightStudio.Tactic;

namespace LightStudio.PokemonBattle.Data
{
  public static class TempLearnSet
  {
    private static HashSet<int>[] data;
    
    public static void Load(string fileName)
    {
      using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        data = Serializer.Deserialize<HashSet<int>[]>(fs);
    }
    public static IEnumerable<int> GetMoves(int number)
    {
      return data.ValueOrDefault(number - 1) ?? Enumerable.Empty<int>();
    }
  }
}
