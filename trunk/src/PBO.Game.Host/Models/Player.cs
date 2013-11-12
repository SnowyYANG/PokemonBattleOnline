using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host
{
  internal class Player
  {
    public readonly int TeamId;
    public readonly int TeamIndex;

    public Player(Controller controller, int teamId, int teamIndex, IPokemonData[] pokemons)
    {
      TeamId = teamId;
      TeamIndex = teamIndex;
      _pokemons = new PokemonProxy[pokemons.Length];
      for (int i = 0; i < pokemons.Length; i++)
        _pokemons[i] = new PokemonProxy(new Pokemon(controller, teamId * 50 + teamIndex * 10 + i, this, pokemons[i]));
    }

    private readonly PokemonProxy[] _pokemons;
    public IEnumerable<PokemonProxy> Pokemons
    { get { return _pokemons; } }
    public int PmsAlive
    { get { return _pokemons.Count((pm) => pm.Hp > 0); } }
    public bool Mega;
    public bool Timing;
    public int SpentTime;

    public PokemonProxy GetPokemon(int pmIndex)
    {
      return _pokemons.ValueOrDefault(pmIndex);
    }
    public int GetPokemonIndex(int pmId)
    {
      for (int i = 0; i < _pokemons.Length; i++)
        if (_pokemons[i].Id == pmId) return i;
      return -1;
    }
    public void SwitchPokemon(int origin, int sendout)
    {
      if (origin >= 0 && origin < _pokemons.Length && sendout >= 0 && sendout < _pokemons.Length)
      {
        var temp = _pokemons[origin];
        _pokemons[origin] = _pokemons[sendout];
        _pokemons[sendout] = temp;
      }
    }
  }
}
