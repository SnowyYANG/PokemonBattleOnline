using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class SimGame
  {
    public readonly Player Player;
    public readonly Team Team;
    public readonly SimPokemon[] OnboardPokemons;
    public readonly IGameSettings Settings;

    public SimGame(int userId, int teamId, PokemonCustomInfo[] pms, IGameSettings settings, Func<int> nextId)
    {
      Team = new Team(teamId, settings, nextId);
      Player = Team.AddPlayer(userId, pms);
      OnboardPokemons = new SimPokemon[settings.Mode.XBound()];
      Settings = settings;
      ActivePokemons = new SortedList<int, SimPokemon>(settings.Mode.XBound());
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
      return ActivePokemons.Count > 0 || (OnboardPokemons.Contains(null) && Player.PmsAlive > OnboardPokemons.Count((p) => p != null));
    }
    /// <summary>
    /// 注意和Update(Turn)的顺序
    /// </summary>
    /// <param name="info"></param>
    public void Update(RequireInput info)
    {
    }
  }
}
