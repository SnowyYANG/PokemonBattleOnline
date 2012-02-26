using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class Team
  {
    public readonly int Id;
    public readonly int PlayerCount;
    public readonly List<Player> Players;
    public readonly Dictionary<int, Pokemon> Pokemons;
    private readonly GameSettings settings;

    public Team(int id, GameSettings settings)
    {
      Id = id;
      this.settings = settings;
      switch (settings.Mode)
      {
        case GameMode.Single:
          PlayerCount = 1;
          break;
      }
      Players = new List<Player>();
      Pokemons = new Dictionary<int, Pokemon>();
    }
    internal bool Prepared
    { get { return Players.Count == PlayerCount; } }

    internal bool AddPlayer(int userId, PokemonCustomInfo[] pokemons)
    {
      if (Players.Count < PlayerCount)
      {
        Players.Add(new Player(userId, this.Id, pokemons, settings));
        if (Players.Count == PlayerCount)
          foreach (Player p in Players)
            foreach (Pokemon pm in p.Pokemons)
              Pokemons.Add(pm.Id, pm);
        return true;
      }
      return false;
    }

    public Player GetPlayer(int userId)
    {
      foreach (Player p in Players)
        if (p.Id == userId) return p;
      return null;
    }

    public TeamOutward GetOutward()
    {
      int normal = 0, abnormal = 0, dying = 0;
      foreach (Pokemon p in Pokemons.Values)
          if (p.Hp.Value == 0) dying++;
          else if (p.State == PokemonState.Normal) normal++;
          else abnormal++;
      return new TeamOutward(normal, abnormal, dying);
    }
  }
}
