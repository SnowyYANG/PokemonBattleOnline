using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game
{
  public enum GameMode : byte
  {
    Single,
    Double,
    Tag,
    Triple,
    Rotation,
    Sky,
    Inverse
  }
  public static class GameModeExtensions
  {
    public static int TeamCount(this GameMode mode)
    {
      return 2;
    }
    public static int XBound(this GameMode mode)
    {
      switch (mode)
      {
        case GameMode.Single:
        case GameMode.Double:
        case GameMode.Triple:
          return 1;
      }
      return 0;
    }
    public static int PlayersPerTeam(this GameMode mode)
    {
      switch (mode)
      {
        case GameMode.Single:
        case GameMode.Double:
        case GameMode.Triple:
          return 1;
        case GameMode.Tag:
          return 2;
      }
      return 0;
    }
    public static int PokemonsPerPlayer(this GameMode mode)
    {
      return 6;
    }
    public static int OnboardPokemonsPerPlayer(this GameMode mode)
    {
      switch (mode)
      {
        case GameMode.Single:
          return 1;
        case GameMode.Double:
          return 2;
        case GameMode.Triple:
          return 3;
        default:
          return 0;
      }
    }
    public static int GetPlayerIndex(this GameMode mode, int x)
    {
      switch (mode)
      {
        case GameMode.Single:
        case GameMode.Double:
        case GameMode.Triple:
          return 0;
      }
      return -1;
    }
    public static int GetPokemonIndex(this GameMode mode, int x)
    {
      switch (mode)
      {
        case GameMode.Single:
          return 0;
      }
      return -1;
    }
    public static int GetPokemonIndexInTeam(this GameMode mode, int x)
    {
      switch (mode)
      {
        case GameMode.Single:
          return 0;
      }
      return -1;
    }
    public static bool NeedTarget(this GameMode mode)
    {
      return mode != GameMode.Single;
    }
  }
}
