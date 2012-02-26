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
  public class Board
  {
    public readonly List<OnboardPokemon> Pokemons;
    public readonly ConditionsDictionary Conditions;
    public readonly ConditionsDictionary[] FieldConditions;
    readonly OnboardPokemon[,] pokemons;
    readonly Terrain terrain;
    readonly GameMode mode;
    Weather weather;

    public Board(GameSettings settings)
    {
      mode = settings.Mode;
      weather = Data.Weather.Normal;
      terrain = settings.Terrain;
      pokemons = new OnboardPokemon[settings.TeamCount, settings.XBound];
      Pokemons = new List<OnboardPokemon>();
      Conditions = new ConditionsDictionary();
      FieldConditions = new ConditionsDictionary[settings.TeamCount];
      for (int i = 0; i < settings.TeamCount; i++) FieldConditions[i] = new ConditionsDictionary();
    }

    public OnboardPokemon this[int team, int x]
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

    public OnboardPokemon GetOnboardPm(int id)
    {
      OnboardPokemon r = null;
      foreach (OnboardPokemon p in Pokemons)
        if (p.Id == id) r = p;
      return r;
    }
    public bool HasAvailableAbility(int abilityId)
    {
      foreach (OnboardPokemon pm in Pokemons)
        if (pm.HasAvailableAbility(abilityId)) return true;
      return false;
    }
    public bool HasAvailableAbility(int teamId, int abilityId)
    {
      foreach (OnboardPokemon pm in Pokemons)
        if (pm.Position.Team == teamId && pm.HasAvailableAbility(abilityId)) return true;
      return false;
    }
  }
}
