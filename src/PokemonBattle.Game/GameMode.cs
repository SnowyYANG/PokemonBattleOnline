using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public enum GameMode : byte
  {
    Single,
    Double,
    Triple,
    //TODO more
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
          return 1;
        default:
          System.Diagnostics.Debugger.Break();
          return 0;
      }
    }
    public static int PlayersPerTeam(this GameMode mode)
    {
      switch (mode)
      {
        case GameMode.Single:
          return 1;
        default:
          System.Diagnostics.Debugger.Break();
          return 0;
      }
    }
    public static int OnboardPokemonsPerPlayer(this GameMode mode)
    {
      switch (mode)
      {
        case GameMode.Single:
          return 1;
        default:
          return 0;
      }
    }
    public static int GetPlayerIndex(this GameMode mode, int x)
    {
      switch (mode)
      {
        case GameMode.Single:
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
    public static bool NeedTarget(this GameMode mode)
    {
      return mode != GameMode.Single;
    }
  }
}
