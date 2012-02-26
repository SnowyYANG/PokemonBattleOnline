using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
    [DataContract(Namespace = Namespaces.DEFAULT)]
    internal class CollectionInfo
    {
        [DataMember]
        public Collection<FolderInfo> Folders
        { get; private set; }

        private CollectionInfo(IEnumerable<FolderInfo> folders)
        {
            this.Folders = new Collection<FolderInfo>();
            foreach (FolderInfo folder in folders)
                this.Folders.Add(folder);
        }

        public static CollectionInfo FromCollection(IPokemonCollection collection)
        {
            return new CollectionInfo(from folder in collection.Folders
                                      select FolderInfo.FromFolder(folder));
        }

        /// <summary>
        /// build a PokemonCollection from the CollectionInfo, excessive items in folders will be removed
        /// </summary>
        public PokemonCollection ToCollection(int folderSize)
        {
            var collection = new PokemonCollection(folderSize, from folderInfo in Folders
                                                               select folderInfo.ToFolder(folderSize));
            return collection;
        }
    }
}
