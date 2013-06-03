using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PokemonDataEditor.ViewModel
{
    public class MenuViewModel
    {
        public string DisplayName
        { get; set; }

        public ObservableCollection<MenuViewModel> Children
        { get; set; }

        public ICommand Command
        { get; set; }

        public MenuViewModel(string displayName, ICommand command)
        {
            DisplayName = displayName;
            Command = command;
            Children = new ObservableCollection<MenuViewModel>();
        }
    }
}
