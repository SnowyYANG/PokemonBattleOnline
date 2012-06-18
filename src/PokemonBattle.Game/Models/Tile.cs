using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public sealed class Tile : ConditionalObject
  {
    public const int NOPM_INDEX = -1;

    public readonly int Team;
    public readonly int X;
    private int speed;

    internal Tile(int team, int x, IGameSettings gameSettings)
    {
      Team = team;
      X = x;
      speed = (team << 3) + x;
      WillSendoutPokemonIndex = gameSettings.Mode.GetPokemonIndex(x);
    }

    public PokemonProxy Pokemon
    { get; internal set; }

    public bool NeedInput
    { get; internal set; }
    public int WillSendoutPokemonIndex
    { get; internal set; }
    public int Speed
    { 
      get
      {
        if (Pokemon != null)
          speed = Pokemon.Speed;
        return speed;
      }
    }
  }
}
