using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class CollectionViewModel : IWeakEventListener
  {
    public IPokemonCollection Model
    { get; private set; }

    public ObservableCollection<FolderViewModel> Folders
    { get; private set; }

    #region Commands

    public MenuCommand NewFolderCommand
    { get; private set; }

    public MenuCommand ImportCommand
    { get; private set; }

    public ObservableCollection<MenuCommand> Commands
    { get; private set; }

    #endregion

    public CollectionViewModel(IPokemonCollection model)
    {
      this.Model = model;
      this.Folders = new ObservableCollection<FolderViewModel>(
          from folder in Model.Folders select new FolderViewModel(this, folder));
      InitializeCommands();

      CollectionChangedEventManager.AddListener(Model.Folders, this);
    }

    private void InitializeCommands()
    {
      if (Model.FolderSize == 6) NewFolderCommand = new MenuCommand("New Team", AddNewFolder);
      else NewFolderCommand = new MenuCommand("New Box", AddNewFolder);
      ImportCommand = new MenuCommand("Import", Import);

      Commands = new ObservableCollection<MenuCommand>();
      Commands.Add(NewFolderCommand);
      Commands.Add(ImportCommand);
    }

    public void AddNewFolder()
    {
      if (Model.FolderSize == 6) Model.AddFolder(DataService.String["new team"]);
      else Model.AddFolder(DataService.String["new box"]);
    }

    public void RemoveFolder(FolderViewModel folder)
    {
      foreach (PokemonViewModel pm in folder.Pokemons)
        pm.Recycle();
      Model.RemoveFolder(folder.Model);
    }

    public void Import()
    {
      try
      {
        FileHelper.OpenFile(DataService.String["XmlFileFilter"],
            stream => Model.ImportFolder(stream));
      }
      catch (Exception)
      {
        UIElements.ShowMessageBox.FolderImportFail();
      }
    }

    #region FoldersChanged

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      if (managerType == typeof(CollectionChangedEventManager) && sender == Model.Folders)
      {
        OnFoldersChanged(e as NotifyCollectionChangedEventArgs);
        return true;
      }
      return false;
    }

    private void OnFoldersChanged(NotifyCollectionChangedEventArgs e)
    {
      if (e.NewItems != null)
      {
        for (int i = 0; i < e.NewItems.Count; i++)
        {
          Folders.Insert(i + e.NewStartingIndex,
              new FolderViewModel(this, e.NewItems[i] as IPokemonFolder));
        }
      }
      if (e.OldItems != null)
      {
        foreach (var item in e.OldItems)
        {
          var folderViewModel = Folders.FirstOrDefault(f => f.Model == item);
          if (folderViewModel == null)
            continue;
          folderViewModel.Close();
          Folders.Remove(folderViewModel);
        }
      }
    }

    #endregion
  }
}
