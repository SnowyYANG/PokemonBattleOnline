using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class Team
  {
    public readonly int Id;
    private readonly IGameSettings Settings;

    internal Team(int id, Player[] players, IGameSettings settings)
    {
      Id = id;
      _players = players;
      Settings = settings;
    }

    private readonly Player[] _players;
    public IEnumerable<Player> Players
    { get { return _players; } }

    public int GetPlayerIndex(int id)
    {
      for (int i = 0; i < _players.Length; ++i)
        if (_players[i].Id == id) return i;
      return -1;
    }
    public Player GetPlayer(int index)
    {
      return _players.ValueOrDefault(index);
    }

    public TeamOutward GetOutward()
    {
      return new TeamOutward(_players, Settings.Mode.PokemonsPerPlayer());
    }
  }
}
