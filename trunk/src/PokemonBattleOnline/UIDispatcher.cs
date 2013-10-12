using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline
{
  public static class UIDispatcher
  {
    private static System.Windows.Threading.Dispatcher wpf;

    public static void Init(System.Windows.Threading.Dispatcher dispatcher)
    {
      wpf = dispatcher;
    }

    public static void Invoke(Action action)
    {
      if (wpf.CheckAccess()) action();
      else wpf.Invoke(action);
    }
    public static void Invoke(Delegate method, params object[] args)
    {
      if (wpf.CheckAccess()) method.DynamicInvoke(args);
      else wpf.Invoke(method, args);
    }
    public static void BeginInvoke(Delegate method, params object[] args)
    {
      wpf.BeginInvoke(method, args);
    }
  }
}
