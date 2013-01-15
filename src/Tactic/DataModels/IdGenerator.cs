using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PokemonBattleOnline
{
  public class IdGenerator
  {
    private int currentId;

    /// <summary>
    /// if idBase is 0, the first number IdGenerator generates will be 1.
    /// </summary>
    /// <param name="idBase"></param>
    public IdGenerator(int idBase = 0)
    {
      currentId = idBase;
    }

    public int NextId()
    {
      int id = Interlocked.Increment(ref currentId);
      return id;
    }
  }
}
