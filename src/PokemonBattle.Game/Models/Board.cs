using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  internal class Board : ConditionalObject
  {
    private readonly Field[] fields;
    private readonly Tile[,] tileMap;
    private readonly Terrain terrain;
    public readonly int TeamCount;
    public readonly int XBound;

    public Board(GameContext game)
    {
      GameSettings settings = game.Settings;
      GameMode mode = settings.Mode;
      TeamCount = settings.TeamCount;
      XBound = settings.XBound;
      Weather = Weather.Normal;
      terrain = settings.Terrain;
      {
        tileMap = new Tile[TeamCount, XBound];
        for (int i = 0; i < TeamCount; i++)
          for (int j = 0; j < XBound; j++)
            tileMap[i, j] = new Tile(i, j, game.Teams[i].Players[settings.GetPlayerIndex(j)]);
      }
      {
        fields = new Field[TeamCount];
        for (int i = 0; i < TeamCount; i++)
          fields[i] = new Field();
      }
    }

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
        if (team >= 0 && team < tileMap.Length && x >= 0 && x < tileMap.Length)
          return tileMap[team, x];
        return null;
      }
    }
    public Weather Weather
    { get; set; }

    public PokemonProxy GetPokemon(int id)
    {
      PokemonProxy r = null;
      foreach (Tile t in tileMap)
        if (t.Pokemon != null && t.Pokemon.Id == id) r = t.Pokemon;
      return r;
    }
  }
}
