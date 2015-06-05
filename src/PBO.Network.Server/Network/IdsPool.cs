using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace PokemonBattleOnline
{
  public class IdsPool
  {
    private readonly ConcurrentBag<int> Pool;
    private int max;

    public IdsPool()
    {
      Pool = new ConcurrentBag<int>();
    }

    public int GetId()
    {
      int id;
      if (!Pool.TryTake(out id) || id == 0) id = Interlocked.Increment(ref max);
      return id;
    }
    public void Push(int id)
    {
      Pool.Add(id);
    }
  }
}
