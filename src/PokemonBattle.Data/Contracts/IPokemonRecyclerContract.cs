using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.IO;

namespace LightStudio.PokemonBattle.Data
{
    [ContractClassFor(typeof(IPokemonRecycler))]
    abstract class IPokemonRecyclerContract : IPokemonRecycler
    {
        public void ChangeSize(int newSize)
        {
            Contract.Requires(newSize > 0);
        }

        public abstract string Name
        { get; set; }

        public abstract int Size
        { get; }

        public abstract bool CanAddPokemon
        { get; }

        public abstract ReadOnlyObservableCollection<PokemonCustomInfo> Pokemons
        { get; }

        public abstract void AddNewPokemon();

        public abstract void AddPokemon(PokemonCustomInfo pm);

        public abstract bool RemovePokemon(PokemonCustomInfo pm);

        public abstract void RemovePokemonAt(int index);

        public abstract void InsertPokemon(int index, PokemonCustomInfo pm);

        public abstract void SetPokemon(int index, PokemonCustomInfo pm);

        public abstract void ClearPokemon();

        public abstract void Trim();

        public abstract void Export(Stream stream);

        public abstract event PropertyChangedEventHandler PropertyChanged;

    }
}
