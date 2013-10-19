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

    public GameContext Complete()
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
    public bool Prepare(int userId, int teamId, int teamIndex, IPokemonData[] pms)
    {
      if (
        playerIds[teamId, teamIndex] == 0 &&
        CheckPokemons(pms) &&
        0 <= teamId && teamId < Settings.Mode.TeamCount() &&
        0 <= teamIndex && teamIndex < Settings.Mode.PlayersPerTeam()
         )
      {
        playerIds[teamId, teamIndex] = userId;
        pokemons[teamId, teamIndex] = pms;
        return true;
      }
      return false;
    }
    public void UnPrepare(int teamId, int teamIndex)
    {
      playerIds[teamId, teamIndex] = 0;
    }
    public IPokemonData[] GetPokemons(int teamId, int teamIndex)
    {
      return playerIds[teamId, teamIndex] == 0 ? null : pokemons[teamId, teamIndex];
    }
  }
}
