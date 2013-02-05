using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Tactic.DataModels
{
  public abstract class ConditionalObject
  {
    private readonly Dictionary<string, object> valueConditions;
    private readonly HashSet<string> markConditions;

    protected ConditionalObject()
    {
      valueConditions = new Dictionary<string, object>();
      markConditions = new HashSet<string>();
    }

    public bool HasCondition(string name)
    {
      return markConditions.Contains(name) || valueConditions.ContainsKey(name);
    }
    public bool AddCondition(string name, object value = null)
    {
      if (value == null) return markConditions.Add(name);
      else
      {
        if (valueConditions.ContainsKey(name)) return false;
        valueConditions[name] = value;
        return true;
      }
    }
    public void SetCondition(string name, object value = null)
    {
      if (value == null) markConditions.Add(name);
      else valueConditions[name] = value;
    }
    public T GetCondition<T>(string name)
    {
      return GetCondition(name, default(T));
    }
    public T GetCondition<T>(string name, T defaultValue)
    {
      object o;
      if (valueConditions.TryGetValue(name, out o) && o is T) return (T)o;
      return defaultValue;
    }
    public bool RemoveCondition(string name)
    {
      return markConditions.Remove(name) || valueConditions.Remove(name);
    }
    public void ClearCondition()
    {
      markConditions.Clear();
      valueConditions.Clear();
    }
  }
}
