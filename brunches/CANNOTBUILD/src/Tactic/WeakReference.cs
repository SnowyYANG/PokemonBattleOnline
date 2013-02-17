using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Tactic
{
  public class WeakReference<T> : WeakReference
  {
    public WeakReference(T obj)
      : base(obj)
    {
    }

    public T TargetOrDefault
    {
      get
      {
        T r = default(T);
        var o = Target; //thread safe
        if (o is T) r = (T)o;
        return r;
      }
    }
  }
}
