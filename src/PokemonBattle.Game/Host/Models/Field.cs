using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class Field : ConditionalObject
  {

    internal Field(IEnumerable<Tile> tiles)
    {
      Tiles = tiles;
    }

    public IEnumerable<Tile> Tiles
    { get; private set; }
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
