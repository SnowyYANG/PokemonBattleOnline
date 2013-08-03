using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class Config : SimpleData
  {
    private static readonly Config Current;

    static Config()
    {
      try
      {
        Current = LoadFromXml<Config>("config.xml");
      }
      catch
      {
        Current = new Config();
      }
    }

    public static T GetValue<T>(string key)
    {
      return GetValue(key, default(T));
    }
    public static T GetValue<T>(string key, T defaultValue)
    {
      var dict = Current.values;
      object r;
      if (!dict.TryGetValue(key, out r))
      {
        dict[key] = defaultValue;
        r = defaultValue;
      }
      return (T)r;
    }
    public static void SetValue(string key, object o) //即使没有DefaultValue也一样设置吧
    {
      Current.values[key] = o;
    }

    [DataMember]
    readonly Dictionary<string, object> values;

    private Config()
    {
      values = new Dictionary<string, object>();
    }
  }
}
