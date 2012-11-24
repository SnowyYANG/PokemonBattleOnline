using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public interface IGameSettings
  {
    GameMode Mode { get; }
    Terrain Terrain { get; }
    bool SleepRule { get; }
    IEnumerable<Rule> Rules { get; }
  }
}
