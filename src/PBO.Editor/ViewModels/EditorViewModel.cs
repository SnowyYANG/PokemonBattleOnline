using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using LightStudio.Tactic.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class EditorViewModel : INotifyPropertyChanged
  {
    private EditingPokemonViewModel _editingPokemon;
    public EditingPokemonViewModel EditingPokemon
    {
      get
      {
        return _editingPokemon;
      }
      private set
      {
        if (_editingPokemon != value)
        {
          _editingPokemon = value;
          OnPropertyChanged("EditingPokemon");
        }
      }
    }

    #region Properties
    public IUserPokemonData Model { get; private set; }
    public CollectionViewModel Boxes { get; private set; }
    public CollectionViewModel Teams { get; private set; }
    public RecyclerViewModel Recycler { get; private set; }
    public ObservableCollection<object> OpenWindows { get; private set; }
    public PokemonSearchViewModel Search { get; private set; }
    #endregion

    public EditorViewModel(IUserPokemonData model)
    {
      this.Model = model;
      this.Boxes = new CollectionViewModel(Model.Boxes);
      this.Teams = new CollectionViewModel(Model.Teams);
      this.Recycler = new RecyclerViewModel(Model.Recycler);
      this.OpenWindows = new ObservableCollection<object>();
      this.Search = new PokemonSearchViewModel();
      this.EditingPokemon = new EditingPokemonViewModel();
    }

    public void OpenFolder(IFolderViewModel folder)
    {
      if (!folder.IsOpen)
      {
        folder.IsOpen = true;
        OpenWindows.Add(folder);
      }
    }

    public void CloseFolder(IFolderViewModel folder)
    {
      if (folder.IsOpen)
      {
        folder.IsOpen = false;
        OpenWindows.Remove(folder);
      }
    }

    public void RecyclePokemon(PokemonViewModel pm)
    {
      Recycler.RecyclePokemon(pm);
    }

    public void EditPokemon(PokemonViewModel pm)
    {
      EditingPokemon.PokemonViewModel = pm;
    }

    public void EndEditing()
    {
      EditingPokemon.ResetToLastSaved();
      EditingPokemon.PokemonViewModel = null;
    }

    public void ShowSearchResult(SearchResultViewModel searchResult)
    {
      if (!OpenWindows.Contains(searchResult))
        OpenWindows.Add(searchResult);
    }

    public void CloseSearchResult(SearchResultViewModel searchResult)
    {
      OpenWindows.Remove(searchResult);
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
  }
}
