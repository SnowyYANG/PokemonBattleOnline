using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline
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
    public static TKey KeyOf<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue value)
    {
      foreach (var p in dict)
        if (p.Value.Equals(value)) return p.Key;
      return default(TKey);
    }
    public static T ValueOrDefault<T>(this IList<T> list, int index)
    {
      if (0 <= index && index < list.Count) return list[index];
      return default(T);
    }
    public static T ValueOrDefault<T>(this T[] array, int index)
    {
      if (0 <= index && index < array.Length) return array[index];
      return default(T);
    }
    public static T[] SubArray<T>(this T[] array, int offset, int count)
    {
      T[] r = new T[count];
      Array.Copy(array, offset, r, 0, count);
      return r;
    }
    public static T[] SubArray<T>(this T[] array, int offset)
    {
      return SubArray(array, offset, array.Length - offset);
    }
    public static int IndexOf<T>(this T[] array, T value)
    {
      for (int i = 0; i < array.Length; ++i)
        if (array[i].Equals(value)) return i;
      return -1;
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
    public static void AppendLine(this StringBuilder sb, params object[] args)
    {
      Append(sb, args);
      sb.AppendLine();
    }
  }
}
