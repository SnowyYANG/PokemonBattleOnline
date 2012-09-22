﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Test
{
  class GameSettings : IGameSettings
  {
    private static readonly Rule[] NORULE = new Rule[0];
    
    public GameMode Mode
    { get { return GameMode.Single; } }
    public Terrain Terrain
    { get { return Terrain.Path; } }
    public double PPUp
    { get { return 1.6; } }
    public IEnumerable<Rule> Rules
    { get { return NORULE; } }

    private readonly IdGenerator idGen = new IdGenerator();
    internal int NextId()
    {
      return idGen.NextId();
    }

  }
}