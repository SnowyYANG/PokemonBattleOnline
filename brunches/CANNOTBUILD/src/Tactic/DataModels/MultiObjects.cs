using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline
{
  /// <summary>
  /// add third/forth when needed, no more than 5, prefer an array
  /// </summary>
  public struct MultiObjects
  {
    public readonly object First;
    public readonly object Second;

    public MultiObjects(object first, object second)
    {
      First = first;
      Second = second;
    }
  }
}
