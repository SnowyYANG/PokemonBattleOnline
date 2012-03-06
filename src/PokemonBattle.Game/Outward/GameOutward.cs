using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  /// <summary>
  /// 只能有一个Listner卡住线程，建议是Subtitle
  /// </summary>
  public interface IGameOutwardEvents
  {
    void EventOccurred(GameEvent e);
  }
  public class GameOutward
  {
    /// <summary>
    /// game start, or an observer
    /// </summary>
    public event System.Action LeapTurn;
    public readonly GameSettings Settings;
    public readonly BoardOutward Board;
    public readonly TeamOutward[] Teams;
    private readonly List<IGameOutwardEvents> listeners;

    public GameOutward(GameSettings settings)
    {
      Settings = settings;
      Board = new BoardOutward(Settings);
      Teams = new TeamOutward[Settings.Mode.TeamCount()];
      for (int t = 0; t < Settings.Mode.TeamCount(); t++)
        Teams[t] = new TeamOutward(6, 0, 0);
      listeners = new List<IGameOutwardEvents>();
    }

    public void Update(ReportFragment turn)
    {
      if (turn.Teams != null)
      {
        for (int t = 0; t < Settings.Mode.TeamCount(); t++)
        {
          Teams[t].Update(turn.Teams[t]);
          for (int x = 0; x < Settings.Mode.XBound(); x++)
            Board[t, x] = turn[t, x];
          Board.Weather = turn.Weather;
        }
        LeapTurn();
      }
      foreach (GameEvent e in turn.Events)
      {
        e.Update(this);
        foreach (IGameOutwardEvents l in listeners)
          l.EventOccurred(e);
      }
    }

    public void AddListner(IGameOutwardEvents listener)
    {
      if (listener != null) listeners.Add(listener);
    }
  }
}
