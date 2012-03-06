using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Game
{
  public sealed class Tile : ConditionalObject
  {
    public readonly int Team;
    public readonly int X;
    private int speed;

    internal Tile(int team, int x)
    {
      Team = team;
      X = x;
      speed = (team << 3) + x;
    }

    public PokemonProxy Pokemon
    { get; internal set; }

    public int Speed
    { 
      get
      {
        if (Pokemon != null && Pokemon.Speed != speed)
          speed = Pokemon.Speed;
        return speed;
      }
    }
  }
}
