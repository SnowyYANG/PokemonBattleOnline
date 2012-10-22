using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class Board : ConditionalObject
  {
    private readonly Field[] fields;
    private readonly Tile[] tiles;
    private readonly Terrain terrain;
    public readonly int TeamCount;
    public readonly int XBound;

    internal Board(GameContext game)
    {
      IGameSettings settings = game.Settings;
      GameMode mode = settings.Mode;
      TeamCount = mode.TeamCount();
      XBound = mode.XBound();
      Weather = Weather.Normal;
      terrain = settings.Terrain;
      {
        tiles = new Tile[TeamCount * XBound];
        fields = new Field[TeamCount];
        int j = 0;
        for (int i = 0; i < TeamCount; i++)
        {
          fields[i] = new Field(i, settings);
          foreach(var t in fields[i].Tiles) tiles[j++] = t;
        }
      }
    }

    public IEnumerable<Field> Fields
    { get { return fields; } }
    public IEnumerable<Tile> Tiles
    { get { return tiles; } }
    public Field this[int team]
    { get { return fields.ValueOrDefault(team); } }
    public Weather Weather
    { get; set; }
  }
}