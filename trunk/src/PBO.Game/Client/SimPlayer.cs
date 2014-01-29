﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game
{
  public class SimPlayer
  {
    public SimPlayer(int team, int indexInTeam, IPokemonData[] pokemons)
    {
      Team = team;
      TeamIndex = indexInTeam;
      _pokemons = new SimPokemon[pokemons.Length];
      for (int i = 0; i < pokemons.Length; i++)
        _pokemons[i] = new SimPokemon(team * 50 + indexInTeam * 10 + i, this, pokemons[i]);
    }

    public int Team
    { get; private set; }
    public int TeamIndex
    { get; private set; }
    
    private readonly SimPokemon[] _pokemons;
    public IEnumerable<SimPokemon> Pokemons
    { get { return _pokemons; } }
    public int PmsAlive
    { get { return _pokemons.Count((pm) => pm.Hp.Value > 0); } }

    public SimPokemon GetPokemon(int pmIndex)
    {
      return _pokemons.ValueOrDefault(pmIndex);
    }
    public int GetPokemonIndex(int pmId)
    {
      for (int i = 0; i < _pokemons.Length; i++)
        if (_pokemons[i].Id == pmId) return i;
      return -1;
    }
    public void SwitchPokemon(int origin, int sendout)
    {
      if (origin >= 0 && origin < _pokemons.Length && sendout >= 0 && sendout < _pokemons.Length)
      {
        SimPokemon temp = _pokemons[origin];
        _pokemons[origin] = _pokemons[sendout];
        _pokemons[sendout] = temp;
      }
    }
  }
}
