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
  public class TurnBuilder
  {
    private GameContext game;
    private Turn lastTurn;
    private Turn turn;
    private int number;

    internal TurnBuilder(GameContext game)
    {
      this.game = game;
    }

    public void NewTurn()
    {
      ContinueTurn();
      turn.AddEvent(new BeginTurn(number++));
    }
    public void ContinueTurn()
    {
      TeamOutward[] t = new TeamOutward[game.Teams.Length];
      for (int i = 0; i < t.Length; i++) t[i] = game.Teams[i].GetOutward();
      PokemonOutward[] p = new PokemonOutward[game.Board.Pokemons.Count];
      for (int i = 0; i < p.Length; i++) p[i] = game.Board.Pokemons[i].GetOutward();
      lastTurn = turn;
      turn = new Turn(t, p, game.Board.Weather);
    }
    public Turn GetTurn()
    {
      return new Turn(turn.Events);
    }
    public Turn GetLeapTurn()
    {
      return turn;
    }

    public void AddSendout(int playerId, int pmId)
    {
      turn.AddEvent(new SendOut(playerId, game.Board.GetOnboardPm(pmId).GetOutward()));
    }
  }
}
