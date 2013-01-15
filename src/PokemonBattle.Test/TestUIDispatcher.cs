using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PokemonBattleOnline.Test
{
  class TestUIDispatcher : IUIDispatcher
  {
    private readonly Dispatcher dispatcher;

    public TestUIDispatcher()
    {
      dispatcher = new Dispatcher("TestUI", true);
    }

    public void Start()
    {
    }
    public void Invoke(Action action)
    {
      dispatcher.Invoke(action);
    }
    public void Invoke(Delegate method, params object[] args)
    {
      dispatcher.Invoke(method, args);
    }
    public void BeginInvoke(Delegate method, params object[] args)
    {
      dispatcher.BeginInvoke(method, args);
    }

    public void Dispose()
    {
      dispatcher.Dispose();
    }
  }
}
