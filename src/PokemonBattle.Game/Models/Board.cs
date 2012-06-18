using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class Board : ConditionalObject
  {
    private readonly Field[] fields;
    private readonly Tile[,] tileMap;
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
        tileMap = new Tile[TeamCount, XBound];
        _tiles = new Tile[TeamCount * XBound];
        int t = 0;
        for (int i = 0; i < TeamCount; i++)
          for (int j = 0; j < XBound; j++)
            tileMap[i, j] = _tiles[t++] = new Tile(i, j, settings);
      }
      {
        fields = new Field[TeamCount];
        for (int i = 0; i < TeamCount; i++)
          fields[i] = new Field();
      }
    }

    private Tile[] _tiles;
    public IEnumerable<Tile> Tiles
    { get { return _tiles; } }
    public Field this[int team]
    { 
      get
      {
        if (team >= 0 && team < fields.Length)
          return fields[team];
        return null;
      }
    }
    public Tile this[int team, int x]
    { 
      get
      {
        if (team >= 0 && team < TeamCount && x >= 0 && x < XBound)
          return tileMap[team, x];
        return null;
      }
    }
    public Weather Weather
    { get; set; }
  }
}
