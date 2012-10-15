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
  internal class GroupVM
  {
    private bool team;
    
    public GroupVM(CollectionGroup model, bool isTeamGroup)
    {
      this.Model = model;
      team = isTeamGroup;
      InitializeCommands();
    }

    public CollectionGroup Model
    { get; private set; }
    public ObservableCollection<MenuCommand> Commands
    { get; private set; }
    public MenuCommand NewCollectionCommand
    { get; private set; }
    public MenuCommand ImportCommand
    { get; private set; }

    private void InitializeCommands()
    {
      if (team) NewCollectionCommand = new MenuCommand("New Team", NewCollection) { Icon = Helper.GetImage(@"Balls/Normal.png") };
      else NewCollectionCommand = new MenuCommand("New Box", NewCollection) { Icon = BoxVM.ICON };
      ImportCommand = new MenuCommand("Import", Import);

      Commands = new ObservableCollection<MenuCommand>();
      Commands.Add(NewCollectionCommand);
      Commands.Add(ImportCommand);
    }

    public void NewCollection()
    {
      if (team) Model.AddCollection(DataService.String["new team"]);
      else Model.AddCollection(DataService.String["new box"]);
    }

    public void Import()
    {
      try
      {
        FileHelper.OpenFile(DataService.String["Xml File(*.xml)|*.xml"], Model.Import);
      }
      catch (Exception)
      {
        UIElements.ShowMessageBox.FolderImportFail();
      }
    }
  }
}
