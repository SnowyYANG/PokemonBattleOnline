﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class SimOnboardPokemon
  {
    public readonly SimPokemon Pokemon;
    public readonly PokemonOutward Outward;
    public int X;

    public int Id
    { get { return Pokemon.Id; } }
    /// <summary>
    /// 虽然是private set，但每个技能还是能set的
    /// </summary>
    public SimMove[] Moves
    { get; private set; }

    internal SimOnboardPokemon(SimPokemon pokemon, PokemonOutward outward)
    {
      Pokemon = pokemon;
      Outward = outward;
      X = outward.Position.X;
      Moves = new SimMove[4];
      for (int i = 0; i < pokemon.Moves.Length; ++i) Moves[i] = new SimMove(pokemon.Moves[i]);
    }

    internal void ChangeMoves(int[] moves)
    {
      int i = -1;
      while (++i < moves.Length) Moves[i] = new SimMove(GameDataService.GetMove(moves[i]));
      while (i < 4) Moves[i++] = null;
    }

    public void ChangeMove(int from, int to)
    {
      for (int i = 0; i < 4; ++i)
        if (Moves[i] != null && Moves[i].Type.Id == from)
        {
          Moves[i] = new SimMove(Data.GameDataService.GetMove(to));
          break;
        }
    }
  }
}
