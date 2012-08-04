using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class SimPokemon
  {
    public readonly Pokemon Pokemon;
    public int X;

    public int Id
    { get { return Pokemon.Id; } }
    /// <summary>
    /// 虽然是private set，但每个技能还是能set的
    /// </summary>
    public SimMove[] Moves
    { get; private set; }

    internal SimPokemon(Pokemon pokemon, PokemonOutward outward)
    {
      Pokemon = pokemon;
      X = outward.Position.X;
      Moves = new SimMove[pokemon.Moves.Length];
      for (int i = 0; i < Moves.Length; i++) Moves[i] = new SimMove(pokemon.Moves[i]);
    }
  }
}
