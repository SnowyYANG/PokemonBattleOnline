using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace LightStudio.Tactic.Messaging
{
  internal class SyncList<T> : SyncObject
  {
    protected Collection<T> List
    { get; private set; }

    public int Count
    {
      get
      {
        return PerformRead(() => List.Count);
      }
    }

    public SyncList()
    {
      List = new Collection<T>();
    }

    public void Add(T item)
    {
      PerformWrite(() => List.Add(item));
    }

    public void AddRange(IEnumerable<T> items)
    {
      PerformWrite(
          () =>
          {
            foreach (var item in items)
            {
              List.Add(item);
            }
          });
    }

    public bool Remove(T item)
    {
      return PerformWrite(() => List.Remove(item));
    }

    public void RemoveRange(IEnumerable<T> items)
    {
      PerformWrite(
          () =>
          {
            foreach (var item in items)
            {
              List.Remove(item);
            }
          });
    }

    public IList<T> GetCopy()
    {
      return PerformRead(() => List.ToList());
    }

    /// <summary>
    /// return a copy of the List and clear items
    /// </summary>
    /// <returns></returns>
    public IList<T> DeItems()
    {
      return PerformWrite(
          () =>
          {
            var copy = List.ToList();
            List.Clear();
            return copy;
          });
    }

    public void Clear()
    {
      PerformWrite(() => List.Clear());
    }

    public bool Contains(T item)
    {
      return PerformRead(() => List.Contains(item));
    }
  }
}
