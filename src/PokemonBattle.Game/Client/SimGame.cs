using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class SimGame
  {
    public readonly GameOutward Outward;
    public readonly Player Player;
    public readonly Team Team;
    public readonly SimPokemon[] OnboardPokemons;

    public SimGame(GameOutward game, int userId, int teamId, IPokemonData[] pms, Func<int> nextId)
    {
      Outward = game;
      Team = new Team(teamId, Outward.Settings, nextId);
      Player = Team.AddPlayer(userId, pms);
      OnboardPokemons = new SimPokemon[Outward.Settings.Mode.XBound()];
    }

    public IGameSettings Settings
    { get { return Outward.Settings; } }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="report"></param>
    /// <returns>RequireInput</returns>
    public void Update(ReportFragment report)
    {
      foreach(GameEvent e in report.Events) e.Update(this);
    }
  }
}
