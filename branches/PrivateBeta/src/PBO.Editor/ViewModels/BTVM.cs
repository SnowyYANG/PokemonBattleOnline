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
  internal abstract class BTVM : CollectionVM
  {
    protected BTVM(PokemonBT model)
      : base(model)
    {
      model.CollectionChanged += (sender, e) => OnPropertyChanged("Effect");
      InitializeCommands();
    }

    private bool _renaming;
    public bool Renaming
    {
      get { return _renaming; }
      protected set
      {
        if (_renaming != value)
        {
          _renaming = value;
          OnPropertyChanged("Renaming");
        }
      }
    }
    public override object Background
    { get { return PBO.UIElements.Brushes.GrayB1; } }

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

    public MenuCommand ExportToClipboardCommand
    { get; private set; }

    #endregion

    private void InitializeCommands()
    {
      RenameCommand = new MenuCommand("Rename", () => Renaming = true);
      RemoveFolderCommand = new MenuCommand("Remove", Remove);
      EndRenamingCommand = new MenuCommand("EndRenaming", () => Renaming = false);
      ExportCommand = new MenuCommand("Export", Export);
      ExportToClipboardCommand = new MenuCommand("导出到剪贴板", ExportToClipboard);

      FolderCommands.Add(RenameCommand);
      FolderCommands.Add(ExportCommand);
      FolderCommands.Add(ExportToClipboardCommand);
      FolderCommands.Add(RemoveFolderCommand);

      NewPokemonCommand = new MenuCommand("NewPokemon", AddNewPokemon);
      PokemonCommands.Insert(0, NewPokemonCommand);
    }
    protected virtual void Remove()
    {
      if (IsOpen) EditorVM.Current.Switch(this);
    }
    public void AddNewPokemon()
    {
      if (Model.CanAdd) Model.Add(new PokemonData(Config.GetValue<int>("Editor_LastPokemonNumber", 1), Config.GetValue<int>("Editor_LastPokemonForm")));
    }
    public void Export()
    {
      try
      {
        FileHelper.SaveFile(DataService.String["Text File(*.txt)|*.txt"], Model.Name, (f, stream) =>
          {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(stream))
              sw.Write(((PokemonBT)Model).Export());
          });
      }
      catch (Exception)
      {
        UIElements.ShowMessageBox.FolderExportFail("无法写入文件。");
      }
    }
    public void ExportToClipboard()
    {
      Clipboard.SetText(((PokemonBT)Model).Export());
    }
  }
}
