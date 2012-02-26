using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class PokemonViewModel : IWeakEventListener, INotifyPropertyChanged, ICloneable
  {
    /// <summary>
    /// 判断的是4的倍数，所以剩余1点或2点将被忽略
    /// </summary>
    public bool HasRemainingEv
    {
      get
      {
        return 508 > Model.HpEv + Model.AtkEv + Model.DefEv
                 + Model.SpeedEv + Model.SpAtkEv + Model.SpDefEv;
      }
    }
    public bool CanLearnMore
    { get { return Model.MoveIds.Count() < 4; } }

    private IFolderViewModel _folder;
    public IFolderViewModel Folder
    {
      get
      {
        return _folder;
      }
      set
      {
        if (_folder != value)
        {
          _folder = value;
          OnPropertyChanged("Folder");
        }
      }
    }

    private PokemonCustomInfo _model;
    public PokemonCustomInfo Model
    {
      get
      {
        return _model;
      }
      set
      {
        if (_model != value)
        {
          _model = value;
          Folder.Model.SetPokemon(Folder.Pokemons.IndexOf(this), value);
          OnPropertyChanged("Model");
          OnPropertyChanged("Icon");
          OnPropertyChanged("HasRemainingEv");
          OnPropertyChanged("CanLearnMore");
        }
      }
    }

    public ImageSource Icon
    { get { return DataService.Image.GetPokemonIcon(Model.PokemonTypeId); } }

    #region Commands

    public MenuCommand EditCommand
    { get; private set; }

    public MenuCommand RemoveCommand
    { get; private set; }

    public ObservableCollection<MenuCommand> Commands
    { get; private set; }

    #endregion

    public PokemonViewModel(IFolderViewModel folderViewModel, PokemonCustomInfo model)
    {
      this.Folder = folderViewModel;
      this._model = model;
      InitializeCommand();

      PropertyChangedEventManager.AddListener(Model, this, "PokemonTypeId");
    }

    private void InitializeCommand()
    {
      EditCommand = new MenuCommand("Edit", () => this.Edit());
      RemoveCommand = new MenuCommand("Remove", RemoveSelf);

      Commands = new ObservableCollection<MenuCommand>();
      Commands.Add(EditCommand);
      Commands.Add(RemoveCommand);
    }

    public void RemoveSelf()
    {
      IFolderViewModel originalFolder = Folder;
      this.Recycle();
      originalFolder.RemovePokemon(this);
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      if (managerType == typeof(PropertyChangedEventManager) && sender == Model)
      {
        OnPropertyChanged("Icon");
        return true;
      }
      return false;
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    public PokemonViewModel Clone()
    {
      return new PokemonViewModel(Folder, Model.Clone());
    }

    object ICloneable.Clone()
    {
      return Clone();
    }
  }
}
