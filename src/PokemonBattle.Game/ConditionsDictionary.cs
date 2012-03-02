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

    public T Get<T>(string key)
    {
      object v = this[key];
      if (v is T) return (T)v;
      return default(T);
    }
    public void Set<T>(string key, T value)
    {
      this[key] = value;
    }
  }
}
