using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Triggers
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
}
