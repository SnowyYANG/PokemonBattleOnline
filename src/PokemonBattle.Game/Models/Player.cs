using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class Player
  {
    public readonly int Id;
    public readonly int TeamId;
    public readonly Pokemon[] Pokemons;

    public Player(int userId, int teamId, PokemonCustomInfo[] pokemons, GameSettings settings)
    {
      Id = userId;
      TeamId = teamId;
      Pokemons = new Pokemon[pokemons.Length];
      for (int i = 0; i < pokemons.Length; i++)
        Pokemons[i] = new Pokemon(this, pokemons[i], settings);
    }
    public int AlivePms
    { get { return Pokemons.Count((pm) => pm.Hp.Value > 0); } }

    public Pokemon GetPokemon(int pmId)
    {
      foreach (Pokemon p in Pokemons)
        if (p.Id == pmId) return p;
      return null;
    }
    public int GetPokemonIndex(Pokemon pm)
    {
      for (int i = 0; i < Pokemons.Length; i++)
        if (Pokemons[i] == pm) return i;
      return -1;
    }
  }
}
