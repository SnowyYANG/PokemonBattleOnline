using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
    internal class Board : ConditionalObject
    {
        private readonly Field[] fields;
        private readonly Tile[] tiles;
        private readonly Terrain terrain;
        public readonly int XBound;
        public Weather Weather;

        internal Board(IGameSettings settings)
        {
            GameMode mode = settings.Mode;
            XBound = mode.XBound();
            Weather = Weather.Normal;
            terrain = settings.Terrain;
            {
                tiles = new Tile[2 * XBound];
                fields = new Field[2];
                int j = 0;
                for (int i = 0; i < 2; i++)
                {
                    fields[i] = new Field(i, settings);
                    foreach (var t in fields[i].Tiles) tiles[j++] = t;
                }
            }
            Pokemons = Enumerable.Empty<PokemonProxy>();
        }

        public IEnumerable<Field> Fields
        { get { return fields; } }
        public IEnumerable<Tile> Tiles
        { get { return tiles; } }
        public Field this[int team]
        { get { return fields.ValueOrDefault(team); } }
        public IEnumerable<PokemonProxy> Pokemons
        { get; private set; }

        public void RefreshPokemons()
        {
            var pms = new List<PokemonProxy>();
            foreach (var t in Tiles)
                if (t.Pokemon != null) pms.Add(t.Pokemon);
            Pokemons = pms;
        }
    }
}