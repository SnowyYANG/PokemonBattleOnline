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
    public readonly Dictionary<int, Pokemon> Pokemons;
    private readonly List<Player> players;
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
      players = new List<Player>();
      Pokemons = new Dictionary<int, Pokemon>();
    }
    public IEnumerable<Player> Players
    { get { return players; } }
    internal bool Prepared
    { get { return players.Count == PlayerCount; } }

    internal Player AddPlayer(int userId, PokemonCustomInfo[] pokemons)
    {
      if (players.Count < PlayerCount)
      {
        Player player = new Player(userId, this.Id, pokemons, settings);
        players.Add(player);
        if (players.Count == PlayerCount)
          foreach (Player p in players)
            foreach (Pokemon pm in p.Pokemons)
              Pokemons.Add(pm.Id, pm);
        return player;
      }
      return null;
    }

    public Player GetPlayer(int index)
    {
      return players.ValueOrDefault(index);
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
