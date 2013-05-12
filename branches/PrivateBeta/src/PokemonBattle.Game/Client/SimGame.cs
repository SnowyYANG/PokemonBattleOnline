using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class SimGame
  {
    public readonly IGameSettings Settings;
    public readonly SimPlayer Player;
    public readonly SimOnboardPokemon[] OnboardPokemons;

    public SimGame(IGameSettings settings, SimPlayer player, IPokemonData[] parner)
    {
      Settings = settings;
      Player = player;
      OnboardPokemons = new SimOnboardPokemon[Settings.Mode.XBound()];
      Pokemons = new Dictionary<int, SimPokemon>();
      foreach (var pm in player.Pokemons) Pokemons.Add(pm.Id, pm);
      //讨厌的parner等做4p再说
    }

    public Dictionary<int, SimPokemon> Pokemons
    { get; private set; }

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
