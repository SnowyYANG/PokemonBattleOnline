using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.PBO.UIElements;

namespace PokemonBattleOnline.PBO.Editor
{
  internal abstract class CollectionVM : ObservableObject
  {
    protected CollectionVM(PokemonCollection model)
    {
      this.Model = model;
      InitializeCommands();
    }

    public abstract object Icon { get; }
    public virtual Thickness IconMargin
    { get { return new Thickness(3, 2, 0, 0); } }
    public abstract object BorderBrush { get; }
    public abstract object Background { get; }
    public abstract object Effect { get; }

    public PokemonCollection Model
    { get; private set; }

    private bool _isOpen;
    public bool IsOpen
    {
      get { return _isOpen; }
      set
      {
        if (_isOpen != value)
        {
          _isOpen = value;
          SwitchCommand.Header = _isOpen ? "Close" : "Open";
          OnPropertyChanged("Icon");
        }
      }
    }

    #region Commands
    public MenuCommand SwitchCommand
    { get; private set; }
    public ObservableCollection<MenuCommand> FolderCommands
    { get; private set; }
    public MenuCommand ClearCommand
    { get; private set; }
    public ObservableCollection<MenuCommand> PokemonCommands
    { get; private set; }
    #endregion

    private void InitializeCommands()
    {
      SwitchCommand = new MenuCommand(IsOpen ? "Close" : "Open", () => EditorVM.Current.Switch(this));

      FolderCommands = new ObservableCollection<MenuCommand>();
      FolderCommands.Add(SwitchCommand);

      ClearCommand = new MenuCommand("Clear", Model.Clear);
      PokemonCommands = new ObservableCollection<MenuCommand>();
      PokemonCommands.Add(ClearCommand);
    }
  }
}
