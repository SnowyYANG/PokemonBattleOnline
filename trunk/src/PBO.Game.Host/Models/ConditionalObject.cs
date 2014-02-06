using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
  internal abstract class ConditionalObject
  {
    private readonly Dictionary<string, object> conditions;
    private readonly HashSet<string> turn;

    protected ConditionalObject()
    {
      conditions = new Dictionary<string, object>();
      turn = new HashSet<string>();
    }

    public bool HasCondition(string name)
    {
      return conditions.ContainsKey(name);
    }
    public bool AddCondition(string name, object value = null)
    {
      if (conditions.ContainsKey(name)) return false;
      conditions[name] = value;
      return true;
    }
    public void SetCondition(string name, object value = null)
    {
      conditions[name] = value;
    }
    public T GetCondition<T>(string name)
    {
      return GetCondition(name, default(T));
    }
    public T GetCondition<T>(string name, T defaultValue)
    {
      object o;
      if (conditions.TryGetValue(name, out o) && o is T) return (T)o;
      return defaultValue;
    }
    public bool RemoveCondition(string name)
    {
      return conditions.Remove(name);
    }
    public bool AddTurnCondition(string name, object value = null)
    {
      if (AddCondition(name, value))
      {
        turn.Add(name);
        return true;
      }
      return false;
    }
    public void SetTurnCondition(string name, object value = null)
    {
      SetCondition(name, value);
      turn.Add(name);
    }
    public Condition GetCondition(string name)
    {
      return GetCondition<Condition>(name);
    }
    public void ClearTurnCondition()
    {
      foreach (string c in turn) RemoveCondition(c);
      turn.Clear();
    }
  }
}
