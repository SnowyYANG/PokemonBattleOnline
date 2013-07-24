using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
{
  internal abstract class ConditionalObject : Tactic.DataModels.ConditionalObject
  {
    private readonly HashSet<string> turnConditions;

    protected ConditionalObject()
      : base()
    {
      turnConditions = new HashSet<string>();
    }

    public bool AddTurnCondition(string name, object value = null)
    {
      if (base.AddCondition(name, value))
      {
        turnConditions.Add(name);
        return true;
      }
      return false;
    }
    public void SetTurnCondition(string name, object value = null)
    {
      base.SetCondition(name, value);
      turnConditions.Add(name);
    }
    public Condition GetCondition(string name)
    {
      return GetCondition<Condition>(name);
    }
    internal void ClearTurnCondition()
    {
      foreach (string c in turnConditions)
        RemoveCondition(c);
      turnConditions.Clear();
    }
  }
}
