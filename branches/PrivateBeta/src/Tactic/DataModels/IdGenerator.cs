using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LightStudio
{
  public class IdGenerator
  {
    private int currentId;

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
