using System;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Data
{
    public interface IUserPokemonData
    {
        IPokemonCollection Boxes { get; }
        IPokemonRecycler Recycler { get; }
        IPokemonCollection Teams { get; }
        int SaveInterval { get; set; }

        void Save();
    }
}
