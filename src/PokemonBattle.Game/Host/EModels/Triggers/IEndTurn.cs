using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Triggers
{
  public interface IEndTurn
  {
    void Execute(Controller controller);
  }
}
