using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
{
  public interface ITrigger
  {
  }
  public interface ICanExecute : ITrigger
  {
    bool Execute(PokemonProxy pm);
  }
  public interface IEndTurn : ITrigger
  {
    void Execute(Controller controller);
  }
  public interface IIsGroundAffectable : ITrigger
  {
    bool Execute(PokemonProxy pm, bool abilityAvailable, bool raiseAbility, bool groundAvailable = true);
  }
}
