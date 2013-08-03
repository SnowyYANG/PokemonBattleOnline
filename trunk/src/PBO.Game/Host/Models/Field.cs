using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
  internal class Field : ConditionalObject
  {
    public readonly int Team;
    private readonly Tile[] tiles;

    internal Field(int team, IGameSettings settings)
    {
      Team = team;
      tiles = new Tile[settings.Mode.XBound()];
      for (int x = 0; x < tiles.Length; ++x) tiles[x] = new Tile(this, x, settings); 
    }

    public Tile this[int x]
    { get { return tiles.ValueOrDefault(x); } }

    public IEnumerable<Tile> Tiles
    { get { return tiles; } }
    public IEnumerable<PokemonProxy> Pokemons
    {
      get
      {
        return
          from t in Tiles
          where t.Pokemon != null
          select t.Pokemon; 
      }
    }

    public IEnumerable<PokemonProxy> GetPokemons(int minX, int maxX)
    {
      return
        from t in Tiles
        where t.X >= minX && t.X < maxX && t.Pokemon != null
        select t.Pokemon;
    }
  }
}
