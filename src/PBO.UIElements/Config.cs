using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.PBO.UIElements
{
  public static class Config
  {
    static readonly Dictionary<string, object> values;
    static readonly Dictionary<string, object> defaultValues;

    static Config()
    {
      values = new Dictionary<string, object>();
      defaultValues = new Dictionary<string, object>();
    }

    public static void Register(string key, object defaultValue)
    {
      defaultValues.Add(key, defaultValue);
    }

    public static T GetValue<T>(string key)
    {
      object o;
      if (!values.TryGetValue(key, out o))
        defaultValues.TryGetValue(key, out o);
      return (T)o;
    }
    public static void SetValue(string key, object o) //即使没有DefaultValue也一样设置吧
    {
      if (values.ContainsKey(key)) values[key] = o;
      else values.Add(key, o);
    }
  }
}
