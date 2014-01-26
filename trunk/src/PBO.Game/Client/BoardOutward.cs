﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PokemonBattleOnline.Game
{
  public class BoardOutward
  {
    public event Action<int, int> PokemonSentOut;
    
    public readonly ReadOnlyObservableCollection<PokemonOutward>[] Pokemons;
    public readonly Terrain Terrain;

    private readonly ObservableCollection<PokemonOutward>[] pokemons;
    private readonly IGameSettings Settings;

    internal BoardOutward(IGameSettings settings)
    {
      Settings = settings;
      Teams = new TeamOutward[Settings.Mode.TeamCount()];
      pokemons = new ObservableCollection<PokemonOutward>[settings.Mode.TeamCount()];
      Pokemons = new ReadOnlyObservableCollection<PokemonOutward>[settings.Mode.TeamCount()];
      _weather = Weather.Normal;
      Terrain = settings.Terrain;

      var empty = new PokemonOutward[settings.Mode.XBound()];
      for (int i = 0; i < settings.Mode.TeamCount(); i++)
      {
        pokemons[i] = new ObservableCollection<PokemonOutward>(empty);
        Pokemons[i] = new ReadOnlyObservableCollection<PokemonOutward>(pokemons[i]);
      }
    }

    public PokemonOutward this[int team, int x]
    {
      get { return pokemons[team][x]; }
      set
      {
        //不一定是PmSendOut
        var old = this[team, x];
        if ((old == null && value == null) || ((old != null && value != null) && (old.Id == value.Id))) return;
        pokemons[team][x] = value;
      }
    }
    private Weather _weather;
    public Weather Weather
    {
      get { return _weather; }
      internal set
      {
        _weather = value;
      }
    }
    public TeamOutward[] Teams
    { get; private set; }

    internal void OnPokemonSentOut(int team, int x)
    {
#if TEST
      if (PokemonSentOut != null)
#endif
        PokemonSentOut(team, x);
    }
  }
}
