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
    private readonly List<SimPokemon> pokemons;

    public SimGame(int userId, int teamId, PokemonCustomInfo[] pms, GameSettings settings)
    {
      Team = new Team(teamId, settings);
      Team.AddPlayer(userId, pms);
      Player = Team.GetPlayer(userId);
      Pokemons = new SimPokemon[settings.XBound];
      pokemons = new List<SimPokemon>();
    }

    public List<SimPokemon> ActivePokemons
    { get { return pokemons; } }

    public void Update(Turn turn)
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
