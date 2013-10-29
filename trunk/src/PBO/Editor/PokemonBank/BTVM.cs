using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Collections.Specialized;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Editor
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
    { get { return SBrushes.GrayB1; } }

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
      if (Model.CanAdd)
      {
        var pm = Config.Current.PokemonNumber;
        if (pm < 1) pm = 1;
        else
        {
          var max = RomData.Pokemons.Count();
          if (pm > max) pm = max;
        }
        Model.Add(new PokemonData(pm, Config.Current.PokemonForm));
      }
    }
    public void Export()
    {
      try
      {
        Helper.SaveFile("文本文件(*.txt)|*.txt", Model.Name, (f, stream) =>
          {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(stream))
              sw.Write(((PokemonBT)Model).Export());
          });
      }
      catch (Exception)
      {
        ShowMessageBox.FolderExportFail("无法写入文件。");
      }
    }
    public void ExportToClipboard()
    {
      Clipboard.SetText(((PokemonBT)Model).Export());
    }
  }
}
