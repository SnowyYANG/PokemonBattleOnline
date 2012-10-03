using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Collections.Specialized;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.PBO.UIElements;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal class FolderViewModel : FolderViewModelBase
  {
    static readonly object TEAMNOPMICON;
    static readonly object TEAMNOPMICONOPEN;
    static readonly object BOXICON;
    static readonly object BOXICONOPEN;
    static FolderViewModel()
    {
      TEAMNOPMICON = PBO.Helper.GetImage(@"Balls/Normal.png");
      TEAMNOPMICONOPEN = PBO.Helper.GetImage(@"Balls/NormalOpen.png");
      BOXICON = PBO.Helper.GetImage(@"Balls/Dive.png");
      BOXICONOPEN = PBO.Helper.GetImage(@"Balls/DiveOpen.png");
    }
    private CollectionViewModel collection;

    public override object BorderBrush
    {
      get
      {
        if (IsTeam) return PBO.UIElements.Brushes.MagentaM;
        return PBO.UIElements.Brushes.BlueM;
      }
    }
    public override object Background
    {
      get
      {
        return PBO.UIElements.Brushes.GrayB1;
      }
    }
    public override object Icon
    {
      get
      {
        if (IsTeam)
        {
          if (IsOpen) return TEAMNOPMICONOPEN;
          var pm = Pokemons.FirstOrDefault();
          if (pm == null) return TEAMNOPMICON;
          else return pm.Icon;
        }
        if (IsOpen) return BOXICONOPEN;
        return BOXICON;
      }
    }
    public override string Name
    {
      get
      {
        return Model.Name;
      }
      set
      {
        if (Model.Name != value)
        {
          Model.Name = value;
          OnPropertyChanged("Name");
        }
      }
    }

    public override bool IsTeam
    { get { return Size == 6; } }
    public override bool IsBox
    { get { return Size != 6; } }

    #region Commands

    public MenuCommand RemoveFolderCommand
    { get; private set; }

    public MenuCommand RenameCommand
    { get; private set; }

    public MenuCommand EndRenamingCommand
    { get; private set; }

    public MenuCommand NewPokemonCommand
    { get; private set; }

    public MenuCommand ExportCommand
    { get; private set; }

    #endregion

    public FolderViewModel(CollectionViewModel collectionViewModel, IPokemonFolder model)
      : base(model)
    {
      this.collection = collectionViewModel;
      InitializeCommands();
    }

    private void InitializeCommands()
    {
      RenameCommand = new MenuCommand("Rename", () => Renaming = true);
      RemoveFolderCommand = new MenuCommand("Remove", () => collection.RemoveFolder(this));
      EndRenamingCommand = new MenuCommand("EndRenaming", () => Renaming = false);
      ExportCommand = new MenuCommand("Export", Export);

      FolderCommands.Add(RenameCommand);
      FolderCommands.Add(ExportCommand);
      FolderCommands.Add(RemoveFolderCommand);

      NewPokemonCommand = new MenuCommand("NewPokemon", AddNewPokemon) { IsEnabled = CanAddPokemon };
      PokemonCommands.Insert(0, NewPokemonCommand);
    }

    public void AddNewPokemon()
    {
      Pokemons.Add(new PokemonViewModel(this, 
        new PokemonCustomInfo(Config.GetValue<int>("Editor_LastPokemonNumber", 1), Config.GetValue<int>("Editor_LastPokemonForme"))));
    }
    public void Export()
    {
      try
      {
        FileHelper.SaveFile(DataService.String["XmlFileFilter"],
            stream => Model.Export(stream));
      }
      catch (Exception)
      {
        UIElements.ShowMessageBox.FolderExportFail();
      }
    }
    protected override void OnPokemonsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnPokemonsChanged(e);
      if (NewPokemonCommand.IsEnabled != CanAddPokemon && IsTeam) OnPropertyChanged("Icon");
      NewPokemonCommand.IsEnabled = CanAddPokemon;
    }
  }
}
