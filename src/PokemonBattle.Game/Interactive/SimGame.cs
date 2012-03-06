using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive
{
  public class SimGame
  {
    public readonly Player Player;
    public readonly Team Team;
    public readonly SimPokemon[] Pokemons;
    public readonly GameSettings Settings;
    private readonly List<SimPokemon> pokemons;

    public SimGame(int userId, int teamId, PokemonCustomInfo[] pms, GameSettings settings)
    {
      Team = new Team(teamId, settings);
      Player = Team.AddPlayer(userId, pms);
      Pokemons = new SimPokemon[settings.Mode.XBound()];
      Settings = settings;
      pokemons = new List<SimPokemon>();
    }

    public List<SimPokemon> ActivePokemons
    { get { return pokemons; } }

    public void Update(ReportFragment turn)
    {
      foreach (GameEvent e in turn.Events)
        e.Update(this);
    }
    /// <summary>
    /// 注意和Update(Turn)的顺序
    /// </summary>
    /// <param name="info"></param>
    public void Update(PokemonAdditionalInfo info)
    {
    }
  }
}
