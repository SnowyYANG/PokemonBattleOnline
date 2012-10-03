using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
    [DataContract(Namespace = Namespaces.PBO)]
    internal class FolderInfo
    {
        [DataMember]
        public string Name
        { get; private set; }

        [DataMember]
        public Collection<PokemonCustomInfo> Pokemons
        { get; private set; }

        private FolderInfo(string name, IEnumerable<PokemonCustomInfo> pokemons)
        {
            this.Name = name;
            this.Pokemons = new Collection<PokemonCustomInfo>();
            foreach (PokemonCustomInfo pm in pokemons)
                this.Pokemons.Add(pm);
        }

        public static FolderInfo FromFolder(IPokemonFolder folder)
        {
            return new FolderInfo(folder.Name, folder.Pokemons);
        }

        /// <summary>
        /// build a PokemonFolder from the FolderInfo, excessive items will be removed
        /// </summary>
        public PokemonFolder ToFolder(int size)
        {
            var folder = new PokemonFolder(Name, size, Pokemons);
            folder.Trim();
            return folder;
        }
    }
}
