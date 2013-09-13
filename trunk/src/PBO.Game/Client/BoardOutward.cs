using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PokemonBattleOnline.Game
{
  public interface IBoardOutwardEvents
  {
    void PokemonSentout(int team, int x);
    void WeatherChanged();
  }

  public class BoardOutward
  {
    public readonly ReadOnlyObservableCollection<PokemonOutward>[] Teams;
    public readonly Terrain Terrain;

    private readonly ObservableCollection<PokemonOutward>[] teams;
    private readonly IGameSettings settings;
    private Weather weather;
    
    private IBoardOutwardEvents listener;

    internal BoardOutward(IGameSettings settings)
    {
      this.settings = settings;
      teams = new ObservableCollection<PokemonOutward>[settings.Mode.TeamCount()];
      Teams = new ReadOnlyObservableCollection<PokemonOutward>[settings.Mode.TeamCount()];
      weather = Weather.Normal;
      Terrain = settings.Terrain;

      var empty = new PokemonOutward[settings.Mode.XBound()];
      for (int i = 0; i < settings.Mode.TeamCount(); i++)
      {
        teams[i] = new ObservableCollection<PokemonOutward>(empty);
        Teams[i] = new ReadOnlyObservableCollection<PokemonOutward>(teams[i]);
      }
    }

    public PokemonOutward this[int team, int x]
    {
      get { return teams[team][x]; }
      set
      {
        //不一定是PmSendout
        var old = this[team, x];
        if ((old == null && value == null) || ((old != null && value != null) && (old.Id == value.Id))) return;
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
#if DEBUG
      if (this.listener != null) System.Diagnostics.Debugger.Break();
#endif
      this.listener = listener;
    }
    public void PokemonSentout(GameOutward game, int team, int x)
    {
      listener.PokemonSentout(team, x);
    }
    public void WeatherChanged()
    {
      listener.WeatherChanged();
    }
    #endregion
  }
}
