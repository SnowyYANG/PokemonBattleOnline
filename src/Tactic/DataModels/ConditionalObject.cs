using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.DataModels
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
    public void SetCondition(string name, object value = null)
    {
      if (value == null) markConditions.Add(name);
      else valueConditions[name] = value;
    }
    public T GetCondition<T>(string name)
    {
      object o;
      if (valueConditions.TryGetValue(name, out o) && o is T) return (T)o;
      return default(T);
    }
    public void RemoveCondition(string name)
    {
      if (!markConditions.Remove(name)) valueConditions.Remove(name);
    }
    public void ClearCondition()
    {
      markConditions.Clear();
      valueConditions.Clear();
    }
  }
}
