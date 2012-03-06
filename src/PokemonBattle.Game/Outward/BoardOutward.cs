using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public interface IBoardOutwardEvents
  {
    void PokemonSentout(int team, int x);
    void WeatherChanged();
    void ShowAbility(PokemonOutward pm, Ability ability); //粉色条
    void AbilityChanged(PokemonOutward pm, Ability from, Ability to); //粉色条
  }

  public class BoardOutward
  {
    public readonly ReadOnlyObservableCollection<PokemonOutward>[] Teams;
    public readonly Terrain Terrain;

    private readonly ObservableCollection<PokemonOutward>[] teams;
    private readonly List<PokemonOutward> pokemons;
    private readonly GameSettings settings;
    private Weather weather;
    
    private readonly List<IBoardOutwardEvents> listeners;

    internal BoardOutward(GameSettings settings)
    {
      this.settings = settings;
      teams = new ObservableCollection<PokemonOutward>[settings.Mode.TeamCount()];
      Teams = new ReadOnlyObservableCollection<PokemonOutward>[settings.Mode.TeamCount()];
      pokemons = new List<PokemonOutward>();
      weather = Data.Weather.Normal;
      Terrain = settings.Terrain;

      var empty = new PokemonOutward[settings.Mode.XBound()];
      for (int i = 0; i < settings.Mode.TeamCount(); i++)
      {
        teams[i] = new ObservableCollection<PokemonOutward>(empty);
        Teams[i] = new ReadOnlyObservableCollection<PokemonOutward>(teams[i]);
      }

      listeners = new List<IBoardOutwardEvents>();
    }

    public PokemonOutward this[int team, int x]
    {
      get { return teams[team][x]; }
      set
      {
        //不一定是PmSendout
        var old = this[team, x];
        if ((old == null && value == null) || ((old != null && value != null) && (old.Id == value.Id))) return;
        if (old != null) pokemons.Remove(old);
        pokemons.Add(value);
        teams[team][x] = value;
      }
    }
    public Weather Weather
    {
      get { return weather; }
      internal set
      {
        weather = value;
        WeatherChanged();
      }
    }

    #region Events
    public void AddListener(IBoardOutwardEvents listener)
    {
      listeners.Add(listener);
    }
    public void PokemonSentout(int team, int x)
    {
      foreach (IBoardOutwardEvents l in listeners)
        l.PokemonSentout(team, x);
    }
    public void WeatherChanged()
    {
      foreach (IBoardOutwardEvents l in listeners)
        l.WeatherChanged();
    }
    public void ShowAbility(PokemonOutward pokemon, Ability ability)
    {
      foreach (IBoardOutwardEvents l in listeners)
        l.ShowAbility(pokemon, ability);
    }
    public void AbilityChanged(PokemonOutward pokemon, Ability from, Ability to)
    {
      foreach (IBoardOutwardEvents l in listeners)
        l.AbilityChanged(pokemon, from, to);
    }
    #endregion
  }
}
