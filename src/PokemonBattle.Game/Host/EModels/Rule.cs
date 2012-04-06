using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public interface IRule
  {
    bool CanChangeState(PokemonProxy pm, PokemonState state);
  }
  
  public class Rule : GameElement, IRule
  {
    public Rule(int id, string name, string description) : base(id)
    {
      Name = name;
      Description = description;
    }
    public new string Description
    { get; private set; }

    public virtual bool CanChangeState(PokemonProxy pm, PokemonState state)
    {
      return true;
    }
  }

  internal class CombinedRule : IRule
  {
    private List<IRule> rules;
    
    public CombinedRule(IEnumerable<int> ids)
    {
      rules = new List<IRule>(ids.Count());
      foreach (int i in ids)
      {
        Rule r = GameService.GetRule(i);
        if (r != null) rules.Add(r);
      }
    }

    public bool CanChangeState(PokemonProxy pm, PokemonState state)
    {
      foreach (IRule r in rules)
        if (!r.CanChangeState(pm, state)) return false;
      return true;
    }
  }
}
