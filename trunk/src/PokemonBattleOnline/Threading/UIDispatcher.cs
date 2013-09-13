using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline
{
  public interface IUIDispatcher : IDisposable
  {
    void Start();
    void Invoke(Action action);
    void Invoke(Delegate method, params object[] args);
    void BeginInvoke(Delegate method, params object[] args);
  }
  public static class UIDispatcher
  {
    private static IUIDispatcher current;

    public static void Init(IUIDispatcher dispatcher)
    {
      if (UIDispatcher.current == null)
        UIDispatcher.current = dispatcher;
    }

    public static void Invoke(Action action)
    {
      current.Invoke(action);
    }
    public static void Invoke(Delegate method, params object[] args)
    {
      current.Invoke(method, args);
    }
    public static void BeginInvoke(Delegate method, params object[] args)
    {
      current.BeginInvoke(method, args);
    }
  }
}
