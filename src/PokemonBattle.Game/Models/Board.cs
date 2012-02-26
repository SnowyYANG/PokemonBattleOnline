using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  internal class Board
  {
    public readonly List<IPokemonProxy> Pokemons;
    public readonly ConditionsDictionary Conditions;
    public readonly ConditionsDictionary[] FieldConditions;
    readonly PokemonProxy[,] pokemons;
    readonly Terrain terrain;
    readonly GameMode mode;
    Weather weather;

    public Board(GameSettings settings)
    {
      mode = settings.Mode;
      weather = Data.Weather.Normal;
      terrain = settings.Terrain;
      pokemons = new PokemonProxy[settings.TeamCount, settings.XBound];
      Pokemons = new List<IPokemonProxy>();
      Conditions = new ConditionsDictionary();
      FieldConditions = new ConditionsDictionary[settings.TeamCount];
      for (int i = 0; i < settings.TeamCount; i++) FieldConditions[i] = new ConditionsDictionary();
    }

    public PokemonProxy this[int team, int x]
    {
      get { return pokemons[team, x]; }
      set
      {
        if (pokemons[team, x] != null)
          Pokemons.Remove(pokemons[team, x]);
        pokemons[team, x] = value;
        if (value != null) Pokemons.Add(pokemons[team, x]);
      }
    }
    public GameMode Mode
    { get { return mode; } }
    public Weather Weather
    { get { return weather; } }

    public PokemonProxy GetPokemon(int id)
    {
      PokemonProxy r = null;
      foreach (PokemonProxy p in Pokemons)
        if (p.Id == id) r = p;
      return r;
    }
  }
}
