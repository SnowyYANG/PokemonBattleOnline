using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
    [DataContract(Namespace = Namespaces.LIGHT)]
    internal class RecyclerInfo
    {
        [DataMember]
        public int Size
        { get; private set; }
        
        [DataMember]
        public Collection<PokemonCustomInfo> Pokemons
        { get; private set; }

        private RecyclerInfo(int size, IEnumerable<PokemonCustomInfo> pokemons)
        {
            this.Size = size;
            this.Pokemons = new Collection<PokemonCustomInfo>();
            foreach (PokemonCustomInfo pm in pokemons)
                this.Pokemons.Add(pm);
        }

        public static RecyclerInfo FromRecycler(IPokemonRecycler recycler)
        {
            return new RecyclerInfo(recycler.Size, recycler.Pokemons);
        }

        /// <summary>
        /// build a PokemonRecycler from the RecyclerInfo, excessive items will be removed
        /// </summary>
        public PokemonRecycler ToRecycle()
        {
            var recycler = new PokemonRecycler(Size, Pokemons);
            recycler.Trim();
            return recycler;
        }
    }
}
