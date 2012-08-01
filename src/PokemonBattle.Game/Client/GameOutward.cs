﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  /// <summary>
  /// 只能有一个Listner卡住线程，建议是Subtitle
  /// </summary>
  public interface IGameOutwardEvents
  {
    void EventOccurred(GameEvent e);
  }
  public class GameOutward : IFormatProvider, ICustomFormatter
  {
    /// <summary>
    /// game start, or an observer
    /// </summary>
    public event Action LeapTurn;
    public readonly IGameSettings Settings;
    public readonly BoardOutward Board;
    public readonly TeamOutward[] Teams;
    private readonly IDictionary<int, string> players;
    private readonly List<IGameOutwardEvents> listeners;
    private RequireInput requireInput;

    public GameOutward(IGameSettings settings, IDictionary<int, string> players)
    {
      Settings = settings;
      Board = new BoardOutward(Settings);
      Teams = new TeamOutward[Settings.Mode.TeamCount()];
      for (int t = 0; t < Settings.Mode.TeamCount(); t++)
        Teams[t] = new TeamOutward(6, 0, 0);
      this.players = players;
      listeners = new List<IGameOutwardEvents>();
    }

    public PokemonOutward GetPokemon(int id)
    {
      foreach (var team in Board.Teams)
        foreach (var pm in team)
          if (pm != null && pm.Id == id) return pm;
      return null;
    }
    public void Update(ReportFragment turn)
    {
      if (turn.Teams != null)
        UIDispatcher.Invoke(() =>
          {
            for (int t = 0; t < Settings.Mode.TeamCount(); t++)
            {
              Teams[t].Update(turn.Teams[t]);
              for (int x = 0; x < Settings.Mode.XBound(); x++)
                Board[t, x] = turn[t, x];
              Board.Weather = turn.Weather;
            }
            LeapTurn();
          });
      foreach (GameEvent e in turn.Events)
      {
        System.Threading.Thread.Sleep(500);
        UIDispatcher.Invoke(() =>
          {
            e.Update(this);
            foreach (IGameOutwardEvents l in listeners)
              l.EventOccurred(e);
          });
      }
    }
    public void AddListner(IGameOutwardEvents listener)
    {
      if (listener != null) listeners.Add(listener);
    }

    object IFormatProvider.GetFormat(Type formatType)
    {
      if (formatType == typeof(ICustomFormatter)) return this;
      else return null;
    }
    string ICustomFormatter.Format(string format, object arg, IFormatProvider formatProvider)
    {
      string r = null;
      if (arg != null)
      {
        if (arg is int)
        {
          int id = (int)arg;
          switch (format)
          {
            case "P":
              r = players.ValueOrDefault(id);
              break;
            case "pm":
              {
                var pm = GetPokemon(id);
                if (pm != null) r = GetPokemon(id).Name;
              }
              break;
            case "p":
              {
                var pm = GetPokemon(id);
                if (pm != null) r = string.Format(this, DataService.String["{0}'s {1}"], players.ValueOrDefault(pm.OwnerId), pm.Name);
              }
              break;
            case "m":
              r = DataService.GetMove(id).GetLocalizedName();
              break;
            case "a":
              r = DataService.GetAbility(id).GetLocalizedName();
              break;
            case "i":
              r = DataService.GetItem(id).GetLocalizedName();
              break;
          }//switch
        }
        if (r == null) r = arg.ToString();
      }// if (arg != null
      return r;
    }
  }
}
