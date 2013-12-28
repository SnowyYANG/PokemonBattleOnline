﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  public class LvLearnList
  {
    private readonly KeyValuePair<int, int>[][] Lvs;
    private readonly Dictionary<int, KeyValuePair<int, int>[]> Forms;

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
      Eggs[number] = moves;
    }
    public IEnumerable<int> Get(int number)
    {
      return Eggs.ValueOrDefault(number) ?? Enumerable.Empty<int>();
    }
  }
  public class TMHMTutorLearnList
  {
    private int[][] Raw;
    private readonly Dictionary<int, int[]> Forms;

    public TMHMTutorLearnList()
    {
      Raw = new int[RomData.Pokemons.Count()][];
      Forms = new Dictionary<int, int[]>();
    }

    internal void Set(int number, int form, int[] moves)
    {
      if (form == 0) Raw[number - 1] = moves;
      else Forms[number * 100 + form] = moves;
    }
    public IEnumerable<int> Get(int number, int form)
    {
      return (form == 0 ? Raw.ValueOrDefault(number - 1) : Forms.ValueOrDefault(number * 100 + form)) ?? Enumerable.Empty<int>();
    }
  }
  public class SPLearnList
  {
    private readonly Dictionary<int, int[]> SPs;

    public SPLearnList()
    {
      SPs = new Dictionary<int, int[]>();
    }
    internal void Set(int number, int form, int[] moves)
    {
      SPs[number * 100 + form] = moves;
    }
    public IEnumerable<int> Get(int number, int form)
    {
      return SPs.ValueOrDefault(number * 100 + form) ?? Enumerable.Empty<int>();
    }
  }
}
