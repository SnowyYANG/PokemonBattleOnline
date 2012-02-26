using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.IO;

namespace LightStudio.PokemonBattle.Data
{
    internal class PokemonRecycler : IPokemonRecycler
    {
        private ObservableCollection<PokemonCustomInfo> internalPokemons;

        public int Size
        { get; private set; }

        public ReadOnlyObservableCollection<PokemonCustomInfo> Pokemons
        { get; private set; }

        public PokemonRecycler(int size)
            : this(size, Enumerable.Empty<PokemonCustomInfo>())
        { }

        public PokemonRecycler(int size, IEnumerable<PokemonCustomInfo> pokemons)
        {
            Contract.Requires(size > 0);

            this.Size = size;
            this.internalPokemons = new ObservableCollection<PokemonCustomInfo>(pokemons);
            this.Pokemons = new ReadOnlyObservableCollection<PokemonCustomInfo>(internalPokemons);
        }

        public void ChangeSize(int newSize)
        {
            Size = newSize;
            Trim();
            OnPropertyChanged("Size");
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IPokemonFolder

        public string Name
        {
            get
            {
                return "Recycler";
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public bool CanAddPokemon
        {
            get { return true; }
        }

        public void AddPokemon(PokemonCustomInfo pm)
        {
            internalPokemons.Add(pm);
            Trim();
        }

        public bool RemovePokemon(PokemonCustomInfo pm)
        {
            return internalPokemons.Remove(pm);
        }

        public void RemovePokemonAt(int index)
        {
            internalPokemons.RemoveAt(index);
        }

        public void InsertPokemon(int index, PokemonCustomInfo pm)
        {
            internalPokemons.Insert(index, pm);
            Trim();
        }

        public void SetPokemon(int index, PokemonCustomInfo pm)
        {
            internalPokemons[index] = pm;
        }

        public void Trim()
        {
            while (internalPokemons.Count > Size)
                internalPokemons.RemoveAt(0);
        }

        void IPokemonFolder.Export(Stream stream)
        {
            throw new NotSupportedException();
        }

        #endregion


    }
}
