using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace LightStudio.PokemonBattle.Data
{
    [ContractClassFor(typeof(IPokemonFolder))]
    abstract class IPokemonFolderContract : IPokemonFolder
    {
        public abstract string Name
        { get; set; }

        public abstract int Size
        { get; }

        public abstract bool CanAddPokemon
        { get; }

        public abstract ReadOnlyObservableCollection<PokemonCustomInfo> Pokemons
        { get; }

        public void AddNewPokemon()
        {
            Contract.Requires(CanAddPokemon);
        }

        public void AddPokemon(PokemonCustomInfo pm)
        {
            Contract.Requires(pm != null);
            Contract.Requires(CanAddPokemon);
        }

        public void InsertPokemon(int index, PokemonCustomInfo pm)
        {
            Contract.Requires(CanAddPokemon);
        }

        public abstract bool RemovePokemon(PokemonCustomInfo pm);

        public abstract void RemovePokemonAt(int index);

        public abstract void SetPokemon(int index, PokemonCustomInfo pm);

        public void Trim()
        {
            Contract.Ensures(Size >= Pokemons.Count);
        }

        public void Export(Stream stream)
        {
            Contract.Requires(stream != null);
        }

        public abstract void ClearPokemon();

        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
}
