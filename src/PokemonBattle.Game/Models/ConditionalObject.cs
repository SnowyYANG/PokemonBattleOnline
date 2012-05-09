using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
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

    public bool SetTurnCondition(string name, object value = null)
    {
      if (base.SetCondition(name, value))
      {
        turnConditions.Add(name);
        return true;
      }
      return false;
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
      public override void EndTurn() { }
    }
  }
}
