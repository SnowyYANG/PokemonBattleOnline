using System;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;
using System.Diagnostics.Contracts;

namespace LightStudio.PokemonBattle.Data
{
    [ContractClass(typeof(IPokemonFolderContract))]
    public interface IPokemonFolder : INotifyPropertyChanged
    {
        string Name { get; set; }
        int Size { get; }
        bool CanAddPokemon { get; }
        ReadOnlyObservableCollection<PokemonCustomInfo> Pokemons { get; }

        void AddPokemon(PokemonCustomInfo pm);
        bool RemovePokemon(PokemonCustomInfo pm);
        void RemovePokemonAt(int index);
        void InsertPokemon(int index, PokemonCustomInfo pm);
        void SetPokemon(int index, PokemonCustomInfo pm);

        /// <summary>
        /// remove items to ensure the number of items is within the size
        /// </summary>
        void Trim();

        void Export(Stream stream);
    }
}
