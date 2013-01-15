using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace PokemonBattleOnline.PBO
{
  public class WpfDispatcher
  {
    #region static
    static WpfDispatcher current;
    public static WpfDispatcher Current
    { get { return current; } }

    private static bool TryInvoke(Action action)//thread safe
    {
      if (current.dispatcher.CheckAccess())
      {
        action();
        return true;
      }
      return false;
    }
    public static void Init(System.Windows.Threading.Dispatcher dispatcher)
    {
      if (current == null) current = new WpfDispatcher(dispatcher);
    }
    public static void Invoke(Delegate method, params object[] args)
    {
      current.dispatcher.Invoke(method, args);
    }
    public static void Invoke(Action action)
    {
      if (!TryInvoke(action)) current.dispatcher.Invoke(action);
    }
    public static void BeginInvoke(Action action)
    {
      if (!TryInvoke(action)) current.dispatcher.BeginInvoke(action);
    }
    #endregion

    readonly Dictionary<string, object> configs;
    readonly System.Windows.Threading.Dispatcher dispatcher;

    private WpfDispatcher(System.Windows.Threading.Dispatcher dispatcher)
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
  }
}
