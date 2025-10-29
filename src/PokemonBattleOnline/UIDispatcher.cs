using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline
{
  /// <summary>
  /// this should not be used as a dispatcher
  /// </summary>
  public static class UIDispatcher
  {
    private static System.Threading.SynchronizationContext syncContext;

    public static void Init(System.Threading.SynchronizationContext context)
    {
      syncContext = context;
    }

    public static void Invoke(Action action)
    {
#if DEBUG
      try
      {
#endif
        if (action != null)
        {
          if (syncContext == null || System.Threading.SynchronizationContext.Current == syncContext) action();
          else syncContext.Send(_ => action(), null);
        }
#if DEBUG
      }
      catch
      {
        System.Diagnostics.Debugger.Break();
      }
#endif
    }
    public static void Invoke(Delegate method, params object[] args)
    {
#if DEBUG
      try
      {
#endif
        if (method != null)
        {
          if (syncContext == null || System.Threading.SynchronizationContext.Current == syncContext) method.DynamicInvoke(args);
          else syncContext.Send(_ => method.DynamicInvoke(args), null);
        }
#if DEBUG
      }
      catch
      {
        System.Diagnostics.Debugger.Break();
      }
#endif
    }
    public static void BeginInvoke(Delegate method, params object[] args)
    {
#if DEBUG
      try
      {
#endif
        if (method != null)
        {
          if (syncContext == null) method.DynamicInvoke(args);
          else syncContext.Post(_ => method.DynamicInvoke(args), null);
        }
#if DEBUG
      }
      catch
      {
        System.Diagnostics.Debugger.Break();
      }
#endif
    }
  }
}
