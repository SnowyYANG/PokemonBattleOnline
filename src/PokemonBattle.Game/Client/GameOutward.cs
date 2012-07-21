using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public event Action LeapTurn;
    public readonly IGameSettings Settings;
    public readonly BoardOutward Board;
    public readonly TeamOutward[] Teams;
    private readonly List<IGameOutwardEvents> listeners;

    public GameOutward(IGameSettings settings)
    {
      Settings = settings;
      Board = new BoardOutward(Settings);
      Teams = new TeamOutward[Settings.Mode.TeamCount()];
      for (int t = 0; t < Settings.Mode.TeamCount(); t++)
        Teams[t] = new TeamOutward(6, 0, 0);
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
  }
}
