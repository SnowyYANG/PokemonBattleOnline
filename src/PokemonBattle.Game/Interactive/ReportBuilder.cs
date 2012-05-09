using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Interactive
{
  public class ReportBuilder
  {
    private GameContext game;
    private ReportFragment lastLeapFragment;
    private ReportFragment lastFragment;
    private ReportFragment current;

    internal ReportBuilder(GameContext game)
    {
      this.game = game;
      TurnNumber = -1;
    }

    public int TurnNumber
    { get; private set; }

    internal void NewFragment()
    {
      if (current != null)
      {
        lastLeapFragment = current;
        if (lastFragment == null) lastFragment = lastLeapFragment;
        else lastFragment = new ReportFragment(lastLeapFragment);
      }
      
      TeamOutward[] t = new TeamOutward[game.Teams.Length];
      for (int i = 0; i < t.Length; i++) t[i] = game.Teams[i].GetOutward();
      List<PokemonOutward> p = new List<PokemonOutward>();
      for (int i = 0; i < game.Board.TeamCount; i++)
        for (int j = 0; j < game.Board.XBound; j++)
          if (game.Board[i, j].Pokemon != null) p.Add(game.Board[i, j].Pokemon.GetOutward());
      current = new ReportFragment(t, p.ToArray(), game.Board.Weather);
    }
    internal ReportFragment GetFragment()
    {
      return lastFragment;
    }
    internal ReportFragment GetLeapFragment()
    {
      return lastLeapFragment;
    }
    internal void NewTurn()
    {
      Add(new BeginTurn(++TurnNumber));
    }

    public void Add(GameEvent e)
    {
      current.AddEvent(e);
    }
    public void Add(string key, PokemonProxy pm, params string[] data)
    {
      Add(new PmEvent(key, pm, data));
    }
    public void Add(string key)
    {
      Add(new SimpleEvent(key));
    }
  }
}
