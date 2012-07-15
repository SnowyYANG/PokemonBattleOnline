using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public class Team
  {
    public readonly int Id;
    public readonly Dictionary<int, Pokemon> Pokemons;
    private readonly List<Player> players;
    private readonly IGameSettings settings;
    private readonly Func<int> nextId;

    internal Team(int id, IGameSettings settings, Func<int> nextId)
    {
      Id = id;
      this.settings = settings;
      this.nextId = nextId;
      players = new List<Player>();
      Pokemons = new Dictionary<int, Pokemon>();
    }
    public IEnumerable<Player> Players
    { get { return players; } }
    internal bool Prepared
    { get { return players.Count == settings.Mode.PlayersPerTeam(); } }

    internal Player AddPlayer(int userId, Data.PokemonCustomInfo[] pokemons)
    {
      if (players.Count < settings.Mode.PlayersPerTeam() && GetPlayerIndex(userId) == -1)
      {
        Player player = new Player(userId, this, pokemons, settings, nextId);
        players.Add(player);
        if (players.Count == settings.Mode.PlayersPerTeam())
          foreach (Player p in players)
            foreach (Pokemon pm in p.Pokemons)
              Pokemons.Add(pm.Id, pm);
        return player;
      }
      return null;
    }

    public int GetPlayerIndex(int id)
    {
      int i = 0;
      foreach (Player p in players)
      {
        if (p.Id == id) return i;
        ++i;
      }
      return -1;
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
