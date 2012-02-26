using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    internal class SearchResultViewModel
    {
        public IList<PokemonViewModel> Pokemons
        { get; private set; }

        public MenuCommand CloseCommand
        { get; private set; }

        public SearchResultViewModel(IList<PokemonViewModel> result)
        {
            this.Pokemons = result;
            this.CloseCommand = new MenuCommand("Close", () => this.Close());
            ICollectionView view = CollectionViewSource.GetDefaultView(Pokemons);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Folder"));
        }
    }
}
