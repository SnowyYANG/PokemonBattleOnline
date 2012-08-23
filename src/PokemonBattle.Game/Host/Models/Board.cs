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
        fields = new Field[TeamCount];
        int t = 0;
        for (int i = 0; i < TeamCount; i++)
        {
          var ts = new Tile[XBound];
          for (int j = 0; j < XBound; j++)
            tileMap[i, j] = _tiles[t++] = ts[j] = new Tile(i, j, settings);
          fields[i] = new Field(ts);
        }
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

    internal void ClearAllElementsTurnConditions()
    {
      ClearTurnCondition();
      foreach (var f in fields) f.ClearTurnCondition();
      foreach (var t in Tiles)
      {
        if (t.Pokemon != null) t.Pokemon.OnboardPokemon.ClearTurnCondition();
        t.ClearTurnCondition();
      }
    }
  }
}