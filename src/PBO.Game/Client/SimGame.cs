using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game
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
      if (parner != null)
        for (int i = 0; i < parner.Length; i++)
        {
          var id = player.Team * 50 + (1 - player.TeamIndex) * 10 + i;
          Pokemons.Add(id, new SimPokemon(id, null, parner[i]));
        }
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
      foreach (GameEvent e in report.Events) e.Update(this);
    }
  }
}
