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
    public readonly Player ResponsiblePlayer; //位置交换在4P情况下不可用
    private PokemonProxy pokemon;
    private int speed;

    internal Tile(int team, int x, Player responsiblePlayer)
    {
      Team = team;
      X = x;
      speed = (team << 3) + x;
    }

    public PokemonProxy Pokemon
    {
      get { return pokemon; }
      set
      {
        if (!Object.ReferenceEquals(value, pokemon))
        {
          if (value == null || value.Pokemon.Owner != ResponsiblePlayer) return;
          pokemon = value;
        }
      }
    }
    public Pokemon WillSendoutPokemon
    { get; internal set; }
    public int Speed
    { 
      get
      {
        if (pokemon != null && pokemon.Speed != speed)
          speed = pokemon.Speed;
        return speed;
      }
    }
  }
}
