using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Editor
{
    internal class PokemonVM : ObservableObject
    {
        public PokemonVM(PokemonData model)
        {
            Model = model;
            Commands = new ObservableCollection<MenuCommand>();
            if (Model.Container != UserData.Current.Recycler)
            {
                EditCommand = new MenuCommand("Edit", () => EditorVM.Current.EditPokemon(Model));
                Commands.Add(EditCommand);
            }
            RemoveCommand = new MenuCommand("Remove", () => Model.Container.Remove(Model));
            Commands.Add(RemoveCommand);
        }

        public PokemonData Model
        { get; private set; }

        public Effect Effect
        {
            get
            {
                var ev = Model.Ev;
                return
                  Model.Form.Species.Number != 132 && Model.Moves.Count() != 4 ?
                  R.MagentaShadow :
                  508 > ev.Sum() ?
                  R.OrangeShadow :
                  null;
            }
        }

        public ImageSource Icon
        { get { return ImageService.GetPokemonIcon(Model.Form, Model.Gender); } }

        public MenuCommand EditCommand
        { get; private set; }
        public MenuCommand RemoveCommand
        { get; private set; }
        public ObservableCollection<MenuCommand> Commands
        { get; private set; }
    }
}
