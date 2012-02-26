using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    internal interface IFolderViewModel : INotifyPropertyChanged
    {
        IPokemonFolder Model { get; }
        string Name { get; }
        int Size { get; }
        object Icon { get; }
        ObservableCollection<PokemonViewModel> Pokemons { get; }
        bool CanAddPokemon { get; }
        bool IsOpen { get; set; }
        bool Renaming { get; }

        MenuCommand OpenCommand { get; }
        MenuCommand CloseCommand { get; }
        ObservableCollection<MenuCommand> FolderCommands { get; }
        ObservableCollection<MenuCommand> PokemonCommands { get; }

        void InsertPokemon(int index, PokemonViewModel pm);
        void RemovePokemon(PokemonViewModel pm);
    
    }
}
