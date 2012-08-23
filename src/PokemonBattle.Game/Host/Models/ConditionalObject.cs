using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
{
  public abstract class ConditionalObject : Tactic.DataModels.ConditionalObject
  {
    private readonly static Condition NULL = new NullCondition();

    private readonly HashSet<string> turnConditions;

    protected ConditionalObject()
      : base()
    {
      turnConditions = new HashSet<string>();
    }

    public bool SetCondition(Condition condition)
    {
      return base.SetCondition(condition.Name, condition);
    }
    public bool SetTurnCondition(string name, object value = null)
    {
      if (base.SetCondition(name, value))
      {
        turnConditions.Add(name);
        return true;
      }
      return false;
    }
    public bool SetTurnCondition(Condition condition)
    {
      return SetTurnCondition(condition.Name, condition);
    }
    public Condition GetCondition(string name)
    {
      return GetCondition<Condition>(name) ?? NULL;
    }
    internal void ClearTurnCondition()
    {
      foreach (string c in turnConditions)
        RemoveCondition(c);
      turnConditions.Clear();
    }

    private sealed class NullCondition : Condition
    {
      public NullCondition()
        : base(null, null)
      {
      }

      public new void Remove() { }
    }
  }
}
