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
  internal abstract class FolderViewModelBase : IFolderViewModel, IWeakEventListener
  {
    public abstract object BorderBrush { get; }
    public abstract object Background { get; }
    public abstract object Icon { get; }

    public virtual bool IsTeam
    { get { return false; } }
    public virtual bool IsBox
    { get { return false; } }

    public IPokemonFolder Model
    { get; private set; }
    public virtual string Name
    {
      get
      {
        return Model.Name;
      }
      set
      {
        Model.Name = value;
      }
    }
    public int Size
    {
      get
      {
        return Model.Size;
      }
    }
    public ObservableCollection<PokemonViewModel> Pokemons
    { get; private set; }
    public bool CanAddPokemon
    {
      get
      {
        return Model.CanAddPokemon;
      }
    }

    private bool _isOpen;
    public bool IsOpen
    {
      get
      {
        return _isOpen;
      }
      set
      {
        if (_isOpen != value)
        {
          _isOpen = value;
          OnIsOpenChanged();
          OnPropertyChanged("IsOpen");
          OnPropertyChanged("Icon");
        }
      }
    }

    private bool _renaming;
    public bool Renaming
    {
      get
      {
        return _renaming;
      }
      protected set
      {
        if (_renaming != value)
        {
          _renaming = value;
          OnPropertyChanged("Renaming");
        }
      }
    }

    #region Commands
    public MenuCommand OpenCommand
    { get; private set; }
    public MenuCommand CloseCommand
    { get; private set; }
    public ObservableCollection<MenuCommand> FolderCommands
    { get; private set; }
    public MenuCommand ClearCommand
    { get; private set; }
    public ObservableCollection<MenuCommand> PokemonCommands
    { get; private set; }
    #endregion

    protected FolderViewModelBase(IPokemonFolder model)
    {
      this.Model = model;
      this.Pokemons = new ObservableCollection<PokemonViewModel>(
          from pm in Model.Pokemons select new PokemonViewModel(this, pm));
      InitializeCommands();
      CollectionChangedEventManager.AddListener(Pokemons, this);
    }

    private void InitializeCommands()
    {
      OpenCommand = new MenuCommand("Open", () => this.Open()) { IsEnabled = !IsOpen };
      CloseCommand = new MenuCommand("Close", () => this.Close()) { IsEnabled = IsOpen };

      FolderCommands = new ObservableCollection<MenuCommand>();
      FolderCommands.Add(OpenCommand);
      FolderCommands.Add(CloseCommand);

      ClearCommand = new MenuCommand("Clear", ClearPokemon);
      PokemonCommands = new ObservableCollection<MenuCommand>();
      PokemonCommands.Add(ClearCommand);
    }

    private void OnIsOpenChanged()
    {
      OpenCommand.IsEnabled = !IsOpen;
      CloseCommand.IsEnabled = IsOpen;
    }

    public void InsertPokemon(int index, PokemonViewModel pm)
    {
      Pokemons.Insert(index, pm);
      pm.Folder = this;
    }

    public void RemovePokemon(PokemonViewModel pm)
    {
      Pokemons.Remove(pm);
    }

    private void ClearPokemon()
    {
      foreach (PokemonViewModel pm in Pokemons)
        pm.Recycle();
      Pokemons.Clear();
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged = delegate { };
    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
    #region Pokemons Changed
    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      if (managerType == typeof(CollectionChangedEventManager) && sender == Pokemons)
      {
        OnPokemonsChanged(e as NotifyCollectionChangedEventArgs);
        return true;
      }
      return false;
    }
    protected virtual void OnPokemonsChanged(NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Reset)
      {
        while (Model.Pokemons.Count > 0)//remove all
          Model.RemovePokemonAt(0);
      }
      else
      {
        if (e.OldItems != null)
        {
          for (int i = 0; i < e.OldItems.Count; i++)
          {
            Model.RemovePokemonAt(e.OldStartingIndex + i);
          }
        }
        if (e.NewItems != null)
        {
          for (int i = 0; i < e.NewItems.Count; i++)
          {
            Model.InsertPokemon(i + e.NewStartingIndex,
                (e.NewItems[i] as PokemonViewModel).Model);
          }
        }
      }
    }
    #endregion
  }
}
