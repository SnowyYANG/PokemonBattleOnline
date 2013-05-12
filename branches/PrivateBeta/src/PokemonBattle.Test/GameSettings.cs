using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Test
{
  class GameSettings : IGameSettings
  {
    public GameMode Mode
    { get { return GameMode.Single; } }
    public Terrain Terrain
    { get { return Terrain.Path; } }
    public bool SleepRule
    { get { return true; } }

    private readonly IdGenerator idGen = new IdGenerator();
    internal int NextId()
    {
      return idGen.NextId();
    }

  }
}
