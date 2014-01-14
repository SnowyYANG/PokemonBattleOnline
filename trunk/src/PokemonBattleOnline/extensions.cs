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
      return key == null || !dict.TryGetValue(key, out r) ? defaultValue : r;
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
      return 0 <= index && index < list.Count ? list[index] : default(T);
    }
    public static T ValueOrDefault<T>(this T[] array, int index)
    {
      return 0 <= index && index < array.Length ? array[index] : default(T);
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
    /// <summary>
    /// returns -1 if not found
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int IndexOf<T>(this T[] array, T value)
    {
      for (int i = 0; i < array.Length; ++i)
        if (array[i].Equals(value)) return i;
      return -1;
    }
    public static void Sort<T>(this T[] array, IComparer<T> comparer)
    {
      Array.Sort(array, comparer);
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
    public static Type[] SubClasses(this Type type)
    {
      var types = type.Assembly.GetTypes();
      var stypes = new List<Type>(types.Length);
      if (type.IsInterface)
      {
        foreach (var t in types)
          if (t.GetInterfaces().Contains(type)) stypes.Add(t);
      }
      else
        foreach (var t in types)
          if (t.IsSubclassOf(type)) stypes.Add(t);
      return stypes.ToArray();
    }
    /// <summary>
    /// return 0 if any error occurs
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int ToInt(this string s)
    {
      int i;
      int.TryParse(s, out i);
      return i;
    }
    public static string LineBreak(this string s)
    {
      return s + "\r\n";
    }
  }
}
