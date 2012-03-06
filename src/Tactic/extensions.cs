using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio
{
  public static class extensions
  {
    public static void AddAll<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
      foreach (var item in items)
        collection.Add(item);
    }
    public static void AddAll<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
      foreach (var item in items)
      {
        dict.Add(item);
      }
    }
    public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    {
      TValue value;
      dict.TryGetValue(key, out value);
      return value;
    }
    public static T ValueOrDefault<T>(this IList<T> list, int index)
    {
      if (index >= 0 && index < list.Count) return list[index];
      return default(T);
    }
    public static bool DeValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, out TValue value)
    {
      if (dict.TryGetValue(key, out value))
      {
        dict.Remove(key);
        return true;
      }
      return false;
    }
  }
}
