using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio
{
  public static class extensions
  {
    public static void Add<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
      foreach (var i in items) collection.Add(i);
    }
    public static void Remove<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
      foreach (T i in items) collection.Remove(i);
    }
    public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
    {
      TValue r;
      if (key == null || !dict.TryGetValue(key, out r)) r = defaultValue;
      return r;
    }
    public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    {
      return ValueOrDefault(dict, key, default(TValue));
    }
    public static T ValueOrDefault<T>(this IList<T> list, int index)
    {
      if (index >= 0 && index < list.Count) return list[index];
      return default(T);
    }
    public static bool ArrayEquals(this Array x, Array y)
    {
      if (y == null || x.Length != y.Length) return false;
      for (int i = 0; i < x.Length; ++i)
        if (!x.GetValue(i).Equals(y.GetValue(i))) return false;
      return true;
    }
    public static void Append(this StringBuilder sb, params object[] args)
    {
      foreach (var o in args) sb.Append(o);
    }
  }
}
