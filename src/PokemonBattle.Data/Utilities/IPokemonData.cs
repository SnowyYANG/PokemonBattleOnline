using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
  public interface IPokemonData
  {
    string Name { get; }
    PokemonForm Form { get; }
    int Lv { get; }
    PokemonGender Gender { get; }
    PokemonNature Nature { get; }
    int AbilityIndex { get; }
    int ItemId { get; }
    int Happiness { get; }
    I6D Iv { get; }
    I6D Ev { get; }
    IEnumerable<int> MoveIds { get; }
  }
}
