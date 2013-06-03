using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBattle.PokemonData
{
    public interface IDataProvider
    {
        PokemonType GetTypeData(string name);
        PokemonType GetTypeData(int id);
        MoveData GetMoveData(string name);
        MoveData GetMoveData(int id);
        PokemonData GetPokemonData(int id);
        System.Drawing.Bitmap GetImage(int identity, long position);

        MoveData[] GetAllMoves();
        PokemonType[] GetAllTypes();
        PokemonData[] GetAllPokemons();

        Custom.CustomDataInfo CustomData
        { get; }
        bool PokemonIsRemoved(int identity);
        bool CheckPokemon(PokemonCustomInfo pokemon);

        TeamData GetRandomTeam(Random random);
    }
}
