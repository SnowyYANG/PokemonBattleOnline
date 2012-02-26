using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using LightStudio.Tactic.Serialization;
using System.IO;

namespace LightStudio.PokemonBattle.Data
{
    internal class PokemonFolder : IPokemonFolder
    {
        private ObservableCollection<PokemonCustomInfo> internalPokemons;

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (value.Length > UserDataRules.NameLength)
                            value = value.Substring(0, UserDataRules.NameLength);
                        _name = value;
                    }
                    OnPropertyChanged("Name");
                }
            }
        }

        public int Size
        { get; private set; }

        public bool CanAddPokemon
        {
            get
            {
                return Size > Pokemons.Count;
            }
        }

        public ReadOnlyObservableCollection<PokemonCustomInfo> Pokemons
        { get; private set; }

        public PokemonFolder(string name, int size)
            : this(name, size, Enumerable.Empty<PokemonCustomInfo>())
        { }

        public PokemonFolder(string name, int size, IEnumerable<PokemonCustomInfo> pokemons)
        {
            Contract.Requires(size >= 0);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            this.Name = name;
            this.Size = size;

            this.internalPokemons = new ObservableCollection<PokemonCustomInfo>();
            foreach (PokemonCustomInfo pm in pokemons)
            {
                if (PokemonValidator.Validate(pm))
                    this.internalPokemons.Add(pm);
            }
            this.internalPokemons.CollectionChanged +=
                (sender, e) => OnPropertyChanged("CanAddPokemon");

            this.Pokemons = new ReadOnlyObservableCollection<PokemonCustomInfo>(internalPokemons);
        }

        #region IPokemonFolder

        public void AddPokemon(PokemonCustomInfo pm)
        {
            internalPokemons.Add(pm);
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
        }

        public void SetPokemon(int index, PokemonCustomInfo pm)
        {
            internalPokemons[index] = pm;
        }

        public void Trim()
        {
            while (internalPokemons.Count > Size)
                internalPokemons.RemoveAt(Size);
        }

        public void Export(Stream stream)
        {
            Serializer.Serialize(FolderInfo.FromFolder(this), stream);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
