using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace PokemonBattleOnline.Network
{
  internal class IdsPool
  {
    private readonly ConcurrentStack<int> Pool;
    private int max;

    /// <summary>
    /// if idBase is 0, the first number IdGenerator generates will be 1.
    /// </summary>
    /// <param name="idBase"></param>
    public IdsPool(int idBase = 0)
    {
      Pool = new ConcurrentStack<int>();
      max = idBase;
    }

    public int GetId()
    {
      int id;
      if (!Pool.TryPop(out id)) id = Interlocked.Increment(ref max);
      return id;
    }
    public void Push(int id)
    {
      Pool.Push(id);
    }
  }
}
