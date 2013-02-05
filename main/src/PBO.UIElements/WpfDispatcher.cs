using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace PokemonBattleOnline.PBO
{
  public class WpfDispatcher : IUIDispatcher
  {
    readonly System.Windows.Threading.Dispatcher dispatcher;

    public WpfDispatcher(System.Windows.Threading.Dispatcher dispatcher)
    {
      this.dispatcher = dispatcher;
    }

    void IUIDispatcher.Start()
    {
    }
    void IUIDispatcher.Invoke(Action method)
    {
      if (dispatcher.CheckAccess()) method();
      else dispatcher.Invoke(method);
    }
    void IUIDispatcher.Invoke(Delegate method, params object[] args)
    {
      if (dispatcher.CheckAccess()) method.DynamicInvoke(args);
      else dispatcher.Invoke(method, args);
    }
    void IUIDispatcher.BeginInvoke(Delegate method, params object[] args)
    {
      dispatcher.BeginInvoke(method, args);
    }
    void IDisposable.Dispose()
    {
    }
  }
}
