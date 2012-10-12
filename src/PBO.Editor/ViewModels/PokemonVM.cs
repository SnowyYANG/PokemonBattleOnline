using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class PokemonVM : INotifyPropertyChanged
  {
    static PokemonVM()
    {
    }
    
    public PokemonVM(PokemonData model)
    {
      Model = model;
      InitializeCommand();
    }

    public PokemonData Model
    { get; private set; }

    public Effect Effect
    { 
      get
      {
        var ev = Model.Ev;
        return
          Model.Form.Type.Number != 132 && Model.MoveIds.Count() != 4 ?
          Resources.MagentaShadow :
          508 > ev.Hp + ev.Atk + ev.Def + ev.Speed + ev.SpAtk + ev.SpDef ?
          Resources.OrangeShadow :
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

    private void InitializeCommand()
    {
      EditCommand = new MenuCommand("Edit", () => EditorVM.Current.EditPokemon(Model));
      RemoveCommand = new MenuCommand("Remove", () => Model.Container.Remove(Model));

      Commands = new ObservableCollection<MenuCommand>();
      Commands.Add(EditCommand);
      Commands.Add(RemoveCommand);
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
  }
}
