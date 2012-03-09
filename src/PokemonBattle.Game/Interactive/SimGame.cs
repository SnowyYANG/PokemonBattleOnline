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

    public SimGame(int userId, int teamId, PokemonCustomInfo[] pms, GameSettings settings)
    {
      Team = new Team(teamId, settings);
      Player = Team.AddPlayer(userId, pms);
      Pokemons = new SimPokemon[settings.Mode.XBound()];
      Settings = settings;
      ActivePokemons = new SortedList<int, SimPokemon>(1);
    }

    //逆鳞/暴走/花瓣舞为2～3回合，回合结束混乱 <-- 需要AdditionalInfo么..
    public SortedList<int, SimPokemon> ActivePokemons
    { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="report"></param>
    /// <returns>RequireInput</returns>
    public bool Update(ReportFragment report)
    {
      foreach(GameEvent e in report.Events)
        e.Update(this);
      return ActivePokemons.Count > 0;
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
