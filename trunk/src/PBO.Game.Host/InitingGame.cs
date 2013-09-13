using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host
{
  public class InitingGame
  {
    private readonly int[,] playerIds;
    private readonly IPokemonData[,][] pokemons;

    public InitingGame(IGameSettings settings)
    {
      _settings = settings;
      playerIds = new int[settings.Mode.TeamCount(), settings.Mode.PlayersPerTeam()];
      pokemons = new IPokemonData[settings.Mode.TeamCount(), settings.Mode.PlayersPerTeam()][];
    }

    private readonly IGameSettings _settings;
    public IGameSettings Settings
    { get { return _settings; } }
    public bool CanComplete
    { 
      get
      { 
        foreach (var i in playerIds)
          if (i == 0) return false;
        return true;
      }
    }

    public IGame Complete()
    {
      if (CanComplete)
      {
        var teams = new Team[Settings.Mode.TeamCount()];
        for (int t = 0; t < teams.Length; ++t)
        {
          var players = new Player[Settings.Mode.PlayersPerTeam()];
          for (int p = 0; p < Settings.Mode.PlayersPerTeam(); ++p) players[p] = new Player(playerIds[t, p], t, p, pokemons[t, p]);
          teams[t] = new Team(t, players, Settings);
        }
        var game = new GameContext(Settings, teams);
        return game;
      }
      return null;
    }
    private bool CheckPokemon(IPokemonData pokemon)
    {
      return PokemonValidator.Validate(pokemon); //TODO: more
    }
    private bool CheckPokemons(IPokemonData[] pokemons)
    {
      return pokemons != null && pokemons.Any() && pokemons.Length <= 6 && pokemons.All(CheckPokemon); //TODO: more
    }
    public bool SetPlayer(int teamId, int userId, IPokemonData[] pokemons)
    {
      if (CheckPokemons(pokemons) && 0 <= teamId && teamId < Settings.Mode.TeamCount())
      {
        for(int i = 0; i < Settings.Mode.PlayersPerTeam(); ++i)
          if (playerIds[teamId, i] == 0)
          {
            playerIds[teamId, i] = userId;
            this.pokemons[teamId, i] = pokemons;
            return true;
          }
      }
      return false;
    }
    public IPokemonData[] GetPokemons(int teamId, int teamIndex)
    {
      return pokemons[teamId, teamIndex];
    }
  }
}
