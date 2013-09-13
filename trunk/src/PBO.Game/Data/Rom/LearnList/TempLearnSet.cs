using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PokemonBattleOnline.Tactic;

namespace PokemonBattleOnline.Data
{
  public static class TempLearnSet
  {
    private static readonly int[] SMEARGLE;
    private static HashSet<int>[] data;

    static TempLearnSet()
    {
      int n = GameDataService.Moves.Count();
      SMEARGLE = new int[n - 2];
      int i = 0;
      for (int m = 1; m <= n; ++m)
        if (m != 165 && m != 448) SMEARGLE[i++] = m;
    }

    public static void Load(string fileName)
    {
      using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        data = Serializer.Deserialize<HashSet<int>[]>(fs);
    }
    public static IEnumerable<int> GetMoves(int number)
    {
      return number == 235 ? SMEARGLE : data.ValueOrDefault(number - 1) ?? Enumerable.Empty<int>();
    }
  }
}
