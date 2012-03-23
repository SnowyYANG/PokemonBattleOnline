using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public interface IMoveE
  {
    MoveType Move { get; }
    void Attach();
    void PreAct();
    void Act();
  }
}