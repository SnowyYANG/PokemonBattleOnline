using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  public class LvLearnList
  {
    public readonly KeyValuePair<int, int>[][] Lvs;
    public readonly Dictionary<int, KeyValuePair<int, int>[]> Forms;

    public LvLearnList()
    {
      Lvs = new KeyValuePair<int, int>[RomData.Pokemons.Count()][]; //怎么想都是件不可思议的事..
      Forms = new Dictionary<int, KeyValuePair<int, int>[]>();
    }

    internal void Set(int number, int form, KeyValuePair<int, int>[] moves)
    {
      if (form == 0) Lvs[number - 1] = moves;
      else Forms[number * 100 + form] = moves;
    }
    public IEnumerable<KeyValuePair<int, int>> Get(int number, int form)
    {
      return form == 0 ? Lvs.ValueOrDefault(number - 1) : Forms.ValueOrDefault(number * 100 + form);
    }
  }
  public class EggLearnList
  {
    private readonly Dictionary<int, int[]> Eggs;

    public EggLearnList()
    {
      Eggs = new Dictionary<int, int[]>();
    }

    internal void Set(int number, int[] moves)
    {
      Eggs[number - 1] = moves;
    }
    public IEnumerable<int> Get(int number)
    {
      return Eggs.ValueOrDefault(number + 1);
    }
  }
  public class TMHMTutorLearnList
  {
    public int[][] Raw;
    public readonly Dictionary<int, int[]> Forms;

    public TMHMTutorLearnList()
    {
      Raw = new int[RomData.Pokemons.Count()][];
      Forms = new Dictionary<int,int[]>();
    }

    internal void Set(int number, int form, int[] moves)
    {
      if (form == 0) Raw[number - 1] = moves;
      else Forms[number * 100 + form] = moves;
    }
    public IEnumerable<int> Get(int number, int form)
    {
      return form == 0 ? Raw.ValueOrDefault(number - 1) : Forms.ValueOrDefault(number * 100 + form);
    }
  }
}
