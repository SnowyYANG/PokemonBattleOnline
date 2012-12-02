using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using Microsoft.Win32;
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
    public MenuCommand ImportFromClipboardCommand
    { get; private set; }

    private void InitializeCommands()
    {
      if (team) NewCollectionCommand = new MenuCommand("New Team", NewCollection) { Icon = Helper.GetImage(@"Balls/Normal.png") };
      else NewCollectionCommand = new MenuCommand("New Box", NewCollection) { Icon = BoxVM.ICON };
      ImportCommand = new MenuCommand("Import", Import);
      ImportFromClipboardCommand = new MenuCommand("从剪贴板导入", ImportFromClipboard);

      Commands = new ObservableCollection<MenuCommand>();
      Commands.Add(NewCollectionCommand);
      Commands.Add(ImportCommand);
      Commands.Add(ImportFromClipboardCommand);
    }

    public void NewCollection()
    {
      if (team) Model.AddCollection(DataService.String["new team"]);
      else Model.AddCollection(DataService.String["new box"]);
    }

    public void Import()
    {
      bool succeed = false;
      var dialog = new OpenFileDialog();
      dialog.Filter = DataService.String["Text File(*.txt)|*.txt"];
      dialog.Multiselect = true;
      if (dialog.ShowDialog() == true)
        foreach (var f in dialog.FileNames)
          try
          {
            using (var sr = new System.IO.StreamReader(f, Encoding.Default, true))
              succeed = Model.Import(System.IO.Path.GetFileNameWithoutExtension(f), sr.ReadToEnd());
            if (!succeed) UIElements.ShowMessageBox.FolderImportFail(string.Format("文件{0}中没有包含可识别的精灵信息。", f));
          }
          catch (Exception)
          {
            UIElements.ShowMessageBox.FolderImportFail(string.Format("文件{0}读取失败。", f));
          }
    }
    public void ImportFromClipboard()
    {
      var str = Clipboard.GetText();
      if (string.IsNullOrWhiteSpace(str)) UIElements.ShowMessageBox.FolderImportFail("剪贴板中没有文本。");
      if (str.Length > 10000) UIElements.ShowMessageBox.FolderImportFail("剪贴板中的文本过长。");
      else if (!Model.Import("剪贴板", str)) UIElements.ShowMessageBox.FolderImportFail("剪贴板文本中没有包含可识别的精灵信息。");
    }
  }
}
