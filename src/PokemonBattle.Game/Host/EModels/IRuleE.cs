using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
{
  public interface IRuleE
  {
    int Id { get; }
    bool CanChangeState(PokemonProxy pm, PokemonState state);
  }
  public class RuleE : IRuleE
  {
    public RuleE(int id)
    {
      Id = id;
    }

    public int Id
    { get; private set; }

    public virtual bool CanChangeState(PokemonProxy pm, PokemonState state)
    {
      return true;
    }
  }
}
