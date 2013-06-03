using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PokemonBattle.PokemonData
{
    public class BattleData
    {
        public static IDataProvider DataProvider
        { get; set; }

        public static PokemonData GetPokemon(int identity)
        {
            if (DataProvider == null) return null;
            return DataProvider.GetPokemonData(identity);
        }

        public static MoveData GetMove(string name)
        {
            if (DataProvider == null) return null;
            return DataProvider.GetMoveData(name);
        }

        public static MoveData GetMove(int id)
        {
            if (DataProvider == null) return null;
            return DataProvider.GetMoveData(id);
        }

        public static string GetMoveName(int id)
        {
            if (DataProvider == null) return string.Empty;
            var move = GetMove(id);
            if (move == null)
                return string.Empty;
            else
                return move.Name;
        }

        public static PokemonType GetTypeData(string name)
        {
            if (DataProvider == null) return null;
            return DataProvider.GetTypeData(name);
        }

        public static PokemonType GetTypeData(int id)
        {
            if (DataProvider == null) return null;
            return DataProvider.GetTypeData(id);
        }

        public static string GetTypeName(int id)
        {
            if (DataProvider == null) return string.Empty;
            var type = GetTypeData(id);
            if (type == null)
                return string.Empty;
            else
                return type.Name;
        }

        public static Bitmap GetImage(int identity, long position)
        {
            if (DataProvider == null) return null;
            return DataProvider.GetImage(identity, position);
        }

        public static MoveData[] GetAllMoves()
        {
            if (DataProvider == null) return null;
            return DataProvider.GetAllMoves();
        }
        public static PokemonType[] GetAllTypes()
        {
            if (DataProvider == null) return null;
            return DataProvider.GetAllTypes();
        }
        public static PokemonData[] GetAllPokemons()
        {
            if (DataProvider == null) return null;
            return DataProvider.GetAllPokemons();
        }
        public static TeamData GetRandomTeam(Random random)
        {
            if (DataProvider == null) return null;
            return DataProvider.GetRandomTeam(random);
        }

        public static Custom.CustomDataInfo CustomData
        {
            get
            {
                if (DataProvider == null) return null;
                return DataProvider.CustomData;
            }
        }
        public static bool PokemonIsRemoved(int identity)
        {
            if (DataProvider == null) return false;
            return DataProvider.PokemonIsRemoved(identity);
        }
        public static bool CheckPokemon(PokemonCustomInfo pokemon)
        {
            if (DataProvider == null) return false;
            return DataProvider.CheckPokemon(pokemon);
        }

        private BattleData()
        {}

    }
}
