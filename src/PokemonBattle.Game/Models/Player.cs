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
    private readonly Pokemon[] pokemons;

    internal Player(int userId, int teamId, PokemonCustomInfo[] pokemons, GameSettings settings)
    {
      Id = userId;
      TeamId = teamId;
      this.pokemons = new Pokemon[pokemons.Length];
      for (int i = 0; i < pokemons.Length; i++)
        this.pokemons[i] = new Pokemon(this, pokemons[i], settings);
    }
    public IEnumerable<Pokemon> Pokemons
    { get { return pokemons; } }
    public int PmsAlive
    { get { return pokemons.Count((pm) => pm.Hp.Value > 0); } }

    public Pokemon GetPokemon(int pmIndex)
    {
      return pokemons.ValueOrDefault(pmIndex);
    }
    public int GetPokemonIndex(int pmId)
    {
      for (int i = 0; i < pokemons.Length; i++)
        if (pokemons[i].Id == pmId) return i;
      return -1;
    }
    public void SwitchPokemon(int origin, int sendout)
    {
      if (origin >= 0 && origin < pokemons.Length && sendout >= 0 && sendout < pokemons.Length)
      {
        Pokemon temp = pokemons[origin];
        pokemons[origin] = pokemons[sendout];
        pokemons[sendout] = temp;
      }
    }
  }
}
