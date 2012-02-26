using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using System.IO;
using LightStudio.Tactic.Serialization;

namespace LightStudio.PokemonBattle.Data
{
    internal class PokemonCollection : IPokemonCollection
    {

        private ObservableCollection<IPokemonFolder> internalFolders;

        public ReadOnlyObservableCollection<IPokemonFolder> Folders
        { get; private set; }

        public int FolderSize
        { get; private set; }

        public PokemonCollection(int folderSize)
            : this(folderSize, Enumerable.Empty<IPokemonFolder>())
        { }

        public PokemonCollection(int folderSize, IEnumerable<IPokemonFolder> folders)
        {
            this.internalFolders = new ObservableCollection<IPokemonFolder>(folders);
            this.Folders = new ReadOnlyObservableCollection<IPokemonFolder>(internalFolders);
            this.FolderSize = folderSize;
        }

        public void AddFolder(string name)
        {
            internalFolders.Add(new PokemonFolder(name, FolderSize));
        }

        public bool RemoveFolder(IPokemonFolder folder)
        {
            return internalFolders.Remove(folder);
        }

        public void ImportFolder(Stream stream)
        {
            FolderInfo folder = Serializer.Deserialize<FolderInfo>(stream);
            internalFolders.Add(folder.ToFolder(FolderSize));
        }
    }
}
