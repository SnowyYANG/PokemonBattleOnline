using System;
using System.IO;
using System.Collections.ObjectModel;

namespace LightStudio.PokemonBattle.Data
{
    public interface IPokemonCollection
    {
        int FolderSize { get; }
        ReadOnlyObservableCollection<IPokemonFolder> Folders { get; }

        void AddFolder(string name);
        bool RemoveFolder(IPokemonFolder folder);
        void ImportFolder(Stream stream);
    }
}
