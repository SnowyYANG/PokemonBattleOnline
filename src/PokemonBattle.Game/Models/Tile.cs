using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Game
{
  public sealed class Tile : ConditionalObject
  {
    public const int NOPM_INDEX = 0;

    public readonly int Team;
    public readonly int X;
    private int speed;

    internal Tile(int team, int x)
    {
      Team = team;
      X = x;
      speed = (team << 3) + x;
      WillSendoutPokemonIndex = x;
    }

    public PokemonProxy Pokemon
    { get; internal set; }

    /// <summary>
    /// 原作输入的过程中精灵就已经交换了，估计Sendout是一触即发的（无法解释按速度的上场顺序），不需要WillSendoutPokemon
    /// </summary>
    public int WillSendoutPokemonIndex
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
