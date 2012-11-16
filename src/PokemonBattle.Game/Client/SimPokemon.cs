using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class SimPokemon : ObservableObject
  {
    public readonly Pokemon Pokemon;
    public readonly PokemonOutward Outward;
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
      Outward = outward;
      X = outward.Position.X;
      Moves = new SimMove[4];
      for (int i = 0; i < pokemon.Moves.Length; ++i) Moves[i] = new SimMove(pokemon.Moves[i]);
    }

    internal void ChangeMoves(int[] moves)
    {
      Moves = new SimMove[moves.Length];
      for (int i = 0; i < moves.Length; ++i) Moves[i] = new SimMove(GameDataService.GetMove(moves[i]));
      OnPropertyChanged("Moves");
    }

    public void ChangeMove(int from, int to)
    {
      for (int i = 0; i < Moves.Length; ++i)
        if (Moves[i].Type.Id == from) Moves[i] = new SimMove(Data.GameDataService.GetMove(to));
    }
  }
}
