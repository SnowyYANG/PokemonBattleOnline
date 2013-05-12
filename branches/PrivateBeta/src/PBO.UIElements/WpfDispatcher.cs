using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace LightStudio.PokemonBattle.PBO
{
  public class WpfDispatcher : IUIDispatcher
  {
    readonly Dictionary<string, object> configs;
    readonly System.Windows.Threading.Dispatcher dispatcher;

    public WpfDispatcher(System.Windows.Threading.Dispatcher dispatcher)
    {
      this.dispatcher = dispatcher;
      dispatcher.ShutdownFinished += SaveConfigs;
      configs = new Dictionary<string, object>();
    }

    void SaveConfigs(object sender, EventArgs e)
    {
      foreach (KeyValuePair<string, object> p in configs)
      {
        XmlSerializer xs = new XmlSerializer(p.Value.GetType());
        xs.Serialize(new FileStream(p.Key, FileMode.Create), p.Value);
      }
    }
    public ConfigType ApplyConfig<ConfigType>(string fileName) where ConfigType : new()
    {
      //avoid to use reflection for performance...
      ConfigType config;
      if (configs.ContainsKey(fileName)) return (ConfigType)configs[fileName];
      try
      {
        XmlSerializer xs = new XmlSerializer(typeof(ConfigType));
        config = (ConfigType)xs.Deserialize(new FileStream(fileName, FileMode.Open));
      }
      catch
      {
        config = new ConfigType();
      }
      configs.Add(fileName, config);
      return config;
    }
    public ConfigType ApplyConfig<ConfigType>() where ConfigType : new()
    {
      return ApplyConfig<ConfigType>(typeof(ConfigType).Name);
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
