using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Interactive
{
  /// <summary>
  /// thread safe?
  /// </summary>
  public class ReportBuilder
  {
    private GameContext game;
    private ReportFragment lastLeapFragment;
    private ReportFragment lastFragment;
    private ReportFragment current;

    internal ReportBuilder(GameContext game)
    {
      this.game = game;
      NewFragment();
    }

    public int TurnNumber
    { get; private set; }

    internal void NewFragment()
    {
      if (current != null)
      {
        lastLeapFragment = current;
        if (lastFragment == null) lastFragment = lastLeapFragment;
        else lastFragment = new ReportFragment(lastFragment.Events);
      }
      
      TeamOutward[] t = new TeamOutward[game.Teams.Length];
      for (int i = 0; i < t.Length; i++) t[i] = game.Teams[i].GetOutward();
      List<PokemonOutward> p = new List<PokemonOutward>();
      foreach(Tile tile in game.Board.Tiles)
        if (tile.Pokemon != null) p.Add(tile.Pokemon.OnboardPokemon.GetOutward());
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

    internal void AddNewTurn()
    {
      current.AddEvent(new BeginTurn(TurnNumber++));
    }
    public void AddSendout(Player player, PokemonProxy pm)
    {
      current.AddEvent(new SendOut(player.Id, pm.OnboardPokemon.GetOutward()));
    }
  }
}
