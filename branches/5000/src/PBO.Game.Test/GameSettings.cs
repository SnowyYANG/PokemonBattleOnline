using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Test
{
  class GameSettings : IGameSettings
  {
    public GameMode Mode
    { get { return GameMode.Single; } }
    public Terrain Terrain
    { get { return Terrain.Path; } }
    public bool SleepRule
    { get { return true; } }
  }
}
