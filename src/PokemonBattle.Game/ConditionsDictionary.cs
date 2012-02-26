using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public class ConditionsDictionary : Dictionary<string, object>
  {
    public new object this[string key]
    {
      get
      {
        object value;
        TryGetValue(key, out value);
        return value;
      }
      set
      {
        base[key] = value;
      }
    }
  }
}
