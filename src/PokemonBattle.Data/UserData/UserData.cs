using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LightStudio.PokemonBattle.Data
{
    public class UserData
    {

        private UserPokemonData _pokemonData;
        public IUserPokemonData PokemonData
        {
            get
            {
                return _pokemonData;
            }
        }

        public UserData()
        {
            _pokemonData = UserPokemonData.Load();
        }

        public void Save()
        {
            _pokemonData.Save();
        }

    }
}
