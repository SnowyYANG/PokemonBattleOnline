using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal abstract class CollectionVM : INotifyPropertyChanged
  {
    protected CollectionVM(PokemonCollection model)
    {
      this.Model = model;
      InitializeCommands();
    }

    public abstract object Icon { get; }
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

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged = delegate { };
    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
  }
}
