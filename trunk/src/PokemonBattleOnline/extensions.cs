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
            return Array.FindIndex(array, t => t.Equals(value));
            //for (int i = 0; i < array.Length; ++i)
            //    if (array[i].Equals(value)) return i;
            //return -1;
        }

        public static int IndexOf<T>(this T[] array, Predicate<T> match)
        {
            return Array.FindIndex(array, match);
            //for (int i = 0; i < array.Length; ++i)
            //    if (array[i].Equals(value)) return i;
            //return -1;
        }

        public static void Sort<T>(this T[] array, IComparer<T> comparer)
        {
            Array.Sort(array, comparer);
        }

        public static bool ArrayEquals<T>(this T[] a, T[] b)
        {
            return (b != null && a.Length == b.Length && a.SequenceEqual(b));
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

            if (type.IsInterface)
            {
                return types.Where(t => t.GetInterfaces().Contains(type)).ToArray();
            }
            else
            {
                return types.Where(t => t.IsSubclassOf(type)).ToArray();
            }
        }

        /// <summary>
        /// return 0 if any error occurs
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt(this string s, int def = 0)
        {
            int i;
            return int.TryParse(s, out i) ? i : def;
        }

        public static string LineBreak(this string s)
        {
            return s + Environment.NewLine;
        }

        public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
        {
            if (action != null)
            {
                foreach (T item in array)
                {
                    action(item);
                }
            }
        }
    }
}
